using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 綁在 Box(Preview) 內的 Detector上
public class DetectElfInBox : MonoBehaviour
{
    // 定義結界箱的範圍
    [SerializeField] float BoxSizeX;
    [SerializeField] float BoxSizeY;
    [SerializeField] float BoxSizeZ;
    [SerializeField] Material matNormal;
    [SerializeField] Material matCold;
    private Vector3 size;
    // 這個collider只綁在stoneElf上，是為了提前偵測玩家的火球攻擊(並進行閃躲)
    private BoxCollider boxCollider;
    private float elapsedTime; 
    private float liveTime=24; 
    private ParticleSystem[] particles;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        liveTime -= Time.deltaTime;
        Collider[] Colliders = Physics.OverlapBox(gameObject.transform.position, new Vector3(BoxSizeX/2,BoxSizeY/2,BoxSizeZ/2), transform.root.rotation);

        if(elapsedTime<=4f)
            foreach(var target in Colliders)
            {
                if(target.CompareTag("Elf"))
                {   
                    particles = target.GetComponentsInChildren<ParticleSystem>();

                    // 不走動
                    target.GetComponent<NavMeshAgent>().isStopped = true;
                    // 動畫速度降低
                    target.GetComponent<Animator>().speed = Mathf.Lerp(1f,0f,elapsedTime/4f);
                    if(particles.Length>0)
                    {
                        target.GetComponent<ThirdElfController>().stopFire = liveTime;
                        for(int i=0;i<particles.Length;i++)
                        {
                            if(particles[i].isPlaying)
                                particles[i].Pause();
                        }
                    }

                    if(target.GetComponent<BossElfController>() || target.GetComponent<FirstElfController>() || target.GetComponent<SecondElfController>())
                    {
                        // material 變色
                        target.transform.Find("Box027").GetComponent<SkinnedMeshRenderer>().material.color = Color.Lerp(Color.white, new Color(112f/255,233f/255,255f/255,255f/255), elapsedTime/4f); 
                        boxCollider = target.GetComponentInChildren<BoxCollider>();
                    }
                    else
                    {
                        // material 變色
                        target.transform.Find("Box027").GetComponent<SkinnedMeshRenderer>().material = matCold;
                        boxCollider = target.GetComponentInChildren<BoxCollider>();                       
                    }

                    // 關閉閃短用collider (所以之後能被玩家擊中)
                    if(boxCollider!=null)
                        boxCollider.enabled = false;
                }
            }
    }

    private void OnEnable() {
        elapsedTime = 0;
    }

    // 該component被disable前 (即時停效果結束)
    private void OnDisable() {
        Collider[] Colliders = Physics.OverlapBox(gameObject.transform.position, new Vector3(BoxSizeX/2,BoxSizeY/2,BoxSizeZ/2), transform.root.rotation);
        foreach(var target in Colliders)
        {
            if(target.CompareTag("Elf"))
            {   
                // 開始走動
                target.GetComponent<NavMeshAgent>().isStopped = false;
                // 動畫正常播放
                target.transform.GetComponent<Animator>().speed = 1f;
                particles = target.GetComponentsInChildren<ParticleSystem>();
                if(particles.Length>0)
                {
                    target.GetComponent<ThirdElfController>().stopFire = -1f; 
                }
                boxCollider = target.GetComponentInChildren<BoxCollider>();
                if(target.GetComponent<BossElfController>() || target.GetComponent<FirstElfController>() || target.GetComponent<SecondElfController>())
                {
                    // material 變色
                    target.transform.Find("Box027").GetComponent<SkinnedMeshRenderer>().material.color = Color.white; 
                }
                else
                {
                    // material 變色
                    target.transform.Find("Box027").GetComponent<SkinnedMeshRenderer>().material = matNormal;             
                }
                // 開啟閃躲用collider
                if(boxCollider!=null)
                    boxCollider.enabled = true;
            }
        }  
    }

    // void OnDrawGizmos()
    // {    
    //         // 上面的 BoxSizeX/2 ， 這邊要用 BoxSizeX ， 範圍才會相同
    //      Gizmos.matrix = Matrix4x4.TRS(gameObject.transform.position, transform.root.rotation, new Vector3(BoxSizeX,BoxSizeY,BoxSizeZ));
    //      Gizmos.color = Color.red;
    //      Gizmos.DrawCube(Vector3.zero, Vector3.one);
    // }
}
