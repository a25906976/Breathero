using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpdateBossPosition : MonoBehaviour
{
    private Animator anim;
    private bool walkPosition;
    private NavMeshAgent agent;
    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        EventManager.stage = 3;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        // JumpDown
        if(animInfo.IsName("JumpDown") && animInfo.normalizedTime>=0.3f)
        {
            if(!walkPosition)
            {
                StartCoroutine(MovePosition(new Vector3(-56.45f,0.17f,-0.07f)));
                walkPosition = true;
            }
           
            else if(animInfo.normalizedTime>=1f)
            {
                BossElfController.BossMode = "Start";
                this.enabled = false;
            }
        } 
    }


    // 跳下來時，移動object position
    IEnumerator MovePosition(Vector3 target)
    {
        Vector3 currentPos = transform.position;
        float elapsedTime = 0f;
        while(elapsedTime<=1f)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(currentPos, target, elapsedTime/1f);
            yield return null;
        }
        agent.enabled = true;
    }
}
