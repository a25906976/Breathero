using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 綁在 ActionForMagicBook 上 (Canvas的parent)
public class BookAction : MonoBehaviour
{
    private void OnEnable() {
        EventManager.BookActionEvent += ActionTrigger;
    }
    private void OnDisable() {
        EventManager.BookActionEvent -= ActionTrigger;
    }

    // 魔法書的移到右邊(Right)和移回正面(Left)
    private void ActionTrigger(string action)
    {
        // tree elf's stage
        if(EventManager.stage==0)
        {
            if(action=="Right")
            {
                transform.GetComponent<Animator>().SetBool("Right",true);
                // 往右移後顯示該對話
                if(EventManager.DialogueState==4)
                {
                    EventManager.StartDialogue(4);
                    EventManager.DialogueState = 5;
                }

            }
            else if(action=="Left")
            {
                transform.GetComponent<Animator>().SetBool("Left",true);
                // 往左移後顯示該對話
                if(EventManager.DialogueState == 11)
                {
                    EventManager.StartDialogue(11);
                    EventManager.DialogueState = 12;
                }
            }
        }
        // stone elf's stage
        else if(EventManager.stage==1)
        {
            if(action=="Right")
            {
                transform.GetComponent<Animator>().SetBool("Right",true);
            }
            else if(action=="Left")
            {
                transform.GetComponent<Animator>().SetBool("Left",true);
                // 往左移後顯示該對話
                if(EventManager.DialogueState == 7)
                {
                    EventManager.StartDialogue(7);
                    EventManager.DialogueState = 8;
                }
            }
        }
    }
}
