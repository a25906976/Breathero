using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 綁在Energy Manager Object 上
public class SwitchMode : MonoBehaviour
{
    // mouth開啟，所有魔法攻擊才能生效
    [SerializeField] GameObject mouth;
    // 顯示/不顯示 能量條
    [SerializeField] GameObject energyBar;
    // 顯示/不顯示 能量條下方的提示
    [SerializeField] GameObject promptPanel;
    // 顯示/不顯示 呼吸教學箭頭
    [SerializeField] GameObject Arrow;
    // 顯示/不顯示 攻擊範圍圈
    [SerializeField] GameObject AttackRange;
    // 顯示/不顯示 魔法書
    // MagicBook都是指PositionForMagicBook (因為其出現位置，會根據Player的位置)
    [SerializeField] GameObject MagicBook;
    // 顯示/不顯示 按Y鍵召喚魔法書提示
    [SerializeField] GameObject Press_Y;

    [SerializeField] GameObject StartCircle;
    [SerializeField] GameObject Little_DialogueUI;
    public static string Mode_Content;
    private bool time_set = false;
    public bool Already = false;


    //musicput
    public AudioManager audioManager;
    

    private void OnEnable()
    {
        EventManager.SwitchModeEvent += SwitchCombatMode;
    }
    private void OnDisable() 
    {
        EventManager.SwitchModeEvent -= SwitchCombatMode;
    }

    private void SwitchCombatMode(string mode)
    {
        Debug.Log("Switch!!");
        //第零關
        if(mode == "None")
        {
            Debug.Log("Current Mode : " + mode);
            // 不顯示魔法書
            MagicBook.SetActive(false);
            // 按Y鍵召喚魔法書關閉 
            Press_Y.gameObject.SetActive(false);
            // 要有攻擊範圍圈關閉
            AttackRange.SetActive(false);
            mouth.SetActive(false);

        }

        // 關卡介紹模式
        else if(mode == "Initial")
        {
            Debug.Log("Current Mode : " + mode);
            
            // 提示按Y鍵召喚魔法書
            Press_Y.gameObject.SetActive(true);
            //BTW 按Y鍵開關召喚收起來的動畫 在EventManager的update那邊

            // 攻擊範圍圈關閉
            AttackRange.SetActive(false);
            // 不顯示呼吸教學箭頭 
            Arrow.SetActive(false);
            // 不能施法
            mouth.SetActive(false);
            

        }

        // 呼吸教學模式
        else if(mode == "Tutorial")
        {
            if (EventManager.stage==0 || EventManager.stage==1 || EventManager.stage == 2)
            {
                MagicBook.GetComponent<Animator>().SetBool("Move_to_door", true);
            }
            else
            {
                MagicBook.GetComponent<Animator>().SetBool("Boss_move_top", true);
            }

            // 按Y鍵召喚魔法書關閉
            Press_Y.gameObject.SetActive(false);
            // 要有能量條
            energyBar.SetActive(true);
            // 攻擊範圍圈關閉
            AttackRange.SetActive(false);
            // 要能施法
            mouth.SetActive(true);
            Debug.Log("Current Mode : " + mode);


        }

        // 戰鬥模式
        else if(mode == "Combat")
        {
            // 要能施法
            mouth.SetActive(true);
            // 要有能量條
            energyBar.SetActive(true);
            // // 要有能量條下方提示
            // promptPanel.SetActive(true);
            // 攻擊範圍圈關閉 (能量條必定從火呼吸開始)
            AttackRange.SetActive(false); 
            // 不顯示呼吸教學箭頭 
            Arrow.SetActive(false);     
            // 不顯示魔法書
            MagicBook.SetActive(false); 
            Debug.Log("Current Mode : " + mode);
        }

        // 戰鬥結束模式
        else if(mode == "Combat_End") 
        {
            
            // 不能施法
            mouth.SetActive(false);
            // 不需能量條
            energyBar.SetActive(false);
            // 不需能量條下方提示
            promptPanel.SetActive(false);
            // 不需呼吸教學箭頭 
            Arrow.SetActive(false);
            // 不需攻擊範圍圈
            AttackRange.SetActive(false);
            //校正回歸大小
            Vector3 objectscale = MagicBook.transform.localScale;
            MagicBook.transform.localScale = new Vector3(objectscale.x / 7, objectscale.y / 7, objectscale.z);
            // 要有魔法書 (對話)
            MagicBook.SetActive(true);
            
            //在前三關，打完三隻怪後，出現最後一段圖、字
            if (EventManager.stage==0 || EventManager.stage==1 || EventManager.stage == 2)
            {
                for (int i = 3; i < Little_DialogueUI.transform.childCount; i++)
                {
                    //先確保全部的dialogue UI已關閉全關掉
                    Little_DialogueUI.transform.GetChild(i).gameObject.SetActive(false);
                }

                Already = true;
                

                // 無情開門。
                EventManager.OpenDoor();
            }

            if (EventManager.stage == 3)
            { 
                for (int i = 3; i < Little_DialogueUI.transform.childCount; i++)
                {
                    //先確保全部的dialogue UI已關閉全關掉
                    Little_DialogueUI.transform.GetChild(i).gameObject.SetActive(false);
                }
                MagicBook.GetComponent<Animator>().SetBool("Move_forward", true);
                EventManager.StartDialogue(6);
                Already = true;

            }
            
            Debug.Log("Current Mode : " + mode);
        }


    }
    private void Update()
    {
        if (EventManager.stage==0 || EventManager.stage==1 || EventManager.stage == 2|| EventManager.stage == 3)
        {
            //打開最後一個UI!
            if (Little_DialogueUI.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Open_finished") && Already == true)
            {
                Little_DialogueUI.transform.GetChild(Little_DialogueUI.transform.childCount-1).gameObject.SetActive(true);
                Already = false;
            } 
        }
    }
}
