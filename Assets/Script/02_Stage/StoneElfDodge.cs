using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 綁在stoneElf Prefab的DetectFireBall上
public class StoneElfDodge : MonoBehaviour
{
    [SerializeField] float teleportDistance=0.5f;
    private float elapsedTime;
    // 左跳
    private Vector3 dodge;
    // 右跳
    private Vector3 dodge2;
    private CapsuleCollider ElfCollider;

    // 僅在tutorial mode用到
    public bool dodged;
    private bool left;

    private void Start() {
        dodge = new Vector3(0f,0f,teleportDistance);
        dodge2 = new Vector3(0f,0f,-teleportDistance);
        ElfCollider = transform.parent.GetComponent<CapsuleCollider>();
    }
    private void OnEnable() {
        GetComponent<BoxCollider>().enabled = true; 
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag=="FireBall")
        {
            // 左右來回跳
            left = left ? false : true;
            dodged = true;
            StartCoroutine(Move(left));
        }
    }

    IEnumerator Move(bool left)
    {
        elapsedTime = 0f;
        Vector3 currentPos = transform.parent.position;
        while(elapsedTime<=0.005f)
        {
            // // 先變成Trigger (免得撞到其他collider)
            ElfCollider.isTrigger = true;
            elapsedTime += Time.deltaTime;
            if(left)
                transform.parent.position =  currentPos - dodge*elapsedTime/0.005f;
            else
                transform.parent.position =  currentPos - dodge2*elapsedTime/0.005f;
            yield return null;
        }
        // elapsedTime = 0f;
        // currentPos = transform.parent.position;
        // yield return new WaitForSeconds(0.5f);
        // while(elapsedTime<=0.005f)
        // {   
        //     elapsedTime += Time.deltaTime;
        //     if(left)
        //         transform.parent.position =  currentPos + dodge*elapsedTime/0.005f;
        //     else
        //         transform.parent.position =  currentPos + dodge2*elapsedTime/0.005f;
        //     yield return null;
        // }
        ElfCollider.isTrigger = false;

        // transform.parent.GetComponent<Animator>().SetBool("Walk",true);
       
    }
}
