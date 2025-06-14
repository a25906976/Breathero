using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 綁在treeElf prefab的弱點上
public class HitAimICON : MonoBehaviour
{
    public int aimIndex;
    private FirstElfController elf;

    private void OnEnable() 
    {
        elf = transform.parent.GetComponent<FirstElfController>();
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "FireBall")
        {
            if(elf.enabled)
            {
                elf.health--;
                if(elf.health==0)
                {
                    elf.state = ElfStates.DEAD;
                    // 死亡時，要把最後一個弱點也消掉
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<CapsuleCollider>().enabled = false;
                    elf.Dead(aimIndex);
                    
                    return;
                }
                else if(elf.health>0)
                {
                    // 被攻擊時，要把打到的弱點消掉
                    elf.BeAttacked(aimIndex);
                    GetComponent<CapsuleCollider>().enabled = false;
                    GetComponent<SpriteRenderer>().enabled = false;
                }

            }

        }  
    }

}
