using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 綁在 FireBall Prefab上
public class FireBallSetting : MonoBehaviour
{
    // 火球的前進速度
    [SerializeField] float speed;
    // 撞到物體的特效
    [SerializeField] GameObject vfxHit;
    Rigidbody rb;
    
    // 因為transform.forward需要先在fire breath那設定好(Mouth的forward)，不要用Awake()
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        // 如果火球未消滅，15秒後自動消失
        Destroy(gameObject,15f);
    }

    // 火球撞到Elf或Elf上的弱點或場景後，火球會消滅
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Elf" || other.gameObject.tag == "Aim" || other.gameObject.tag == "Wall" || other.gameObject.tag == "Shield")
        {
            // 播火球消散特效
            GameObject tmp = Instantiate(vfxHit,transform.position,Quaternion.identity);
            Destroy(tmp,0.4f);
            Destroy(gameObject);
        }

    }


    // // 火球超出攻擊範圍，火球會消滅
    // private void OnTriggerExit(Collider other) {
    //     if(other.gameObject.tag == "AttackRange")
    //     {
    //         // 播火球消散特效
    //         GameObject tmp = Instantiate(vfxHit,transform.position,Quaternion.identity);
    //         Destroy(tmp,0.4f);
    //         Destroy(gameObject);
    //     }          
    // }

}
