using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// 綁在 CenterEyeAnchor Object 上
public class PlayerBeAttacked : MonoBehaviour
{
    [SerializeField] Volume PostVolume;
    // elf進到這個範圍(很靠近玩家(會攻擊玩家)，就播放紅色特效)
    [SerializeField] float AttackedRadius;
    private Vignette VigBlood;

    private bool BloodyPlaying=false;
    
    private void Awake() {
        PostVolume.profile.TryGet<Vignette>(out VigBlood);
    }
    private void Update() {
        if(!BloodyPlaying)
            BeAttacked();
    }

    private void BeAttacked() {
        // elf進到玩家的攻擊範圍內
        Collider[] colliders = Physics.OverlapSphere(transform.position, AttackedRadius);
        foreach(var target in colliders)
        {
            if(target.CompareTag("Elf"))
            {   
                
                Animator anim = target.gameObject.GetComponent<Animator>();

                // tree elf's stage
                if(EventManager.stage==0)
                {
                    FirstElfController treeElf = target.gameObject.GetComponent<FirstElfController>();
                    // elf非死亡狀態(被玩家打倒)，且攻擊玩家的動畫播到一半(即打下去的動作)，則播放紅色特效
                    // 0 是 layer index，即base layer
                    // GetCurrentAnimatorStateInfo(0) 需要當下呼叫才能取得當前資料，AnimationState變數裡的資訊是不會動態變化的
                    if(treeElf.state != ElfStates.DEAD && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                    {
                        StartCoroutine(Bloody());
                        // 特效已觸發
                        BloodyPlaying = true;
                        return;
                    }
                }
                // stone elf's stage
                else if(EventManager.stage==1)
                {
                    SecondElfController stoneElf = target.gameObject.GetComponent<SecondElfController>();
                    if(!BloodyPlaying && stoneElf.state != ElfStates.DEAD && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                    {
                        StartCoroutine(Bloody());
                        BloodyPlaying = true;
                        return;
                    }
                }
                // TODO:: 被fire elf打到的效果?

                // Boss關
                else if(EventManager.stage==3)
                {
                    FirstElfController treeElf;
                    SecondElfController stoneElf;
                    
                    if((treeElf=target.gameObject.GetComponent<FirstElfController>())!=null)
                        if(!BloodyPlaying && treeElf.state != ElfStates.DEAD && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                        {
                            StartCoroutine(Bloody());
                            BloodyPlaying = true;
                            return;
                        }
                    else if((stoneElf=target.gameObject.GetComponent<SecondElfController>())!=null)
                        if(!BloodyPlaying && stoneElf.state != ElfStates.DEAD && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                        {
                            StartCoroutine(Bloody());
                            BloodyPlaying = true;
                            return;
                        }
                }

            }
        }
    }

    // 紅色特效
    private IEnumerator Bloody()
    {
        float elpasedTime = 0f;
        float elpasedDuration1 = 2f;
        // 變紅
        while(elpasedTime<elpasedDuration1)
        {   
            elpasedTime += Time.deltaTime;
            VigBlood.color.Interp(Color.black,Color.red,elpasedTime/elpasedDuration1);
            VigBlood.intensity.Interp(0.4f, 1f, elpasedTime/elpasedDuration1);
            VigBlood.smoothness.Interp(0.2f, 1f, elpasedTime/elpasedDuration1);
            yield return null;
        }
        elpasedTime = 0f;
        // 變回來
        while(elpasedTime<elpasedDuration1)
        {   
            elpasedTime += Time.deltaTime;
            VigBlood.color.Interp(Color.red,Color.black,elpasedTime/elpasedDuration1);
            VigBlood.intensity.Interp(1f,0.4f, elpasedTime/elpasedDuration1);
            VigBlood.smoothness.Interp(1f,0.2f, elpasedTime/elpasedDuration1);
            yield return null;
        }
        // 播完了
        BloodyPlaying = false;
    }

    // 僅在Editor模式顯示
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, AttackedRadius);   
    }

}
