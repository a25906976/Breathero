using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 不小心寫成global
public enum ElfStates {IDLE, TUTORIAL, WALK, ATTACKED, ATTACK, DEAD, CHARGE, COOLDOWN, StandUp, PURE_CHARGE, PURE_FIRE};

// 管理各種Event
// 綁在 EventManager Object 上
public class EventManager : MonoBehaviour
{   
    // 紀錄當前玩家在第幾關 (在tutorialMode.cs中設定)
    public static int stage;

    // 用了 event keyword 
    public static event Action<int> CreateElfEvent;

    // Book移到右邊(或移回正面)
    public static event Action<string> BookActionEvent;

    // 站樁精靈走到攻擊範圍內
    public static event Action<Vector3> ElfMoveEvent;

    // Mode 有 Story、Tutorial、Combat
    public static event Action<string> SwitchModeEvent;

    // 顯示魔法書對話
    public static event Action<int> StartDialogueEvent;

    // 顯示能量條下方的提示
    public static event Action<int> StartPromptEvent;

    // 打開通往下一關的門
    public static event Action DoorOpenEvent;

    // 魔法書下次要進行的對話
    public static int DialogueState;

    //被火精靈AOE攻擊到的閉眼跟睜眼
    public static event Action<bool> DieEvent;

    [SerializeField] OVRInput.Button SummonMagicBookButton;
    
    // MagicBook都是指PositionForMagicBook (因為其出現位置，會根據Player的位置)
    [SerializeField] GameObject MagicBook;

    // 為了偵測火呼吸時，有無精靈在攻擊範圍內，已顯示Prompt
    [SerializeField] AttackRange attackRange;

    [SerializeField] GameObject StartCircle;
    [SerializeField] GameObject Press_Y;

    public static bool open_book = false;

    // 紀錄當前的模式
    public static string currentMode;

    // 每關都從Initial關卡介紹開始
    private void Start() {
        currentMode = "Initial";
    }

    // number指要創建的精靈數量
    public static void CreateElf(int number)
    {
        CreateElfEvent?.Invoke(number);
    }

    // action為Right或Left
    public static void MagicBookAction(string action)
    {
        BookActionEvent?.Invoke(action);
    }

    // target表示elf要移動到的位置
    public static void TutoralElfMove(Vector3 target)
    {
        ElfMoveEvent?.Invoke(target);
    }
    
    // mode為Story、Tutorial或Combat
    public static void SwitchPlayMode(string mode)
    {
        currentMode = mode;
        SwitchModeEvent?.Invoke(mode);
    }
    
    // state為要顯示的對話的index
    public static void StartDialogue(int state)
    {
        StartDialogueEvent?.Invoke(state);
    }

    // state為要顯示的提示的index
    public static void StartPrompt(int state)
    {
        StartPromptEvent?.Invoke(state);
    }

    public static void OpenDoor()
    {
        DoorOpenEvent?.Invoke();
    }

    //死掉function (state: "true"->睜眼 / "false"->閉眼)
    public static void Die(bool state)
    {
        DieEvent?.Invoke(state);
    }

    private void Update()
    {
        if (currentMode == "Combat")
        {
            // 按按鍵 喚出 MagicBook
            if (OVRInput.GetDown(SummonMagicBookButton, OVRInput.Controller.LTouch))
            {
                if (!MagicBook.activeSelf)
                    MagicBook.SetActive(true);
                else
                    MagicBook.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Close", true);
            }



            // 先改成不要提示
            // // 施展火呼吸時
            // if(ChangeSkill.currentSkill==0)
            //     // 能量條下方顯示 Can Attack
            //     if(attackRange.targetInRange){
            //         StartPrompt(10);
            //     }
            //     // 能量條下方顯示 No Target
            //     else if(!attackRange.targetInRange)
            //     {
            //         StartPrompt(11);
            //     }
        }
        if (currentMode == "Initial")
        {

            // 按按鍵 喚出 MagicBook
            if (OVRInput.GetDown(SummonMagicBookButton, OVRInput.Controller.LTouch) || open_book == true)
            {
                if (!MagicBook.activeSelf)
                {
                    Debug.Log("Press Y!! or Opened!!");
                    MagicBook.SetActive(true);
                    MagicBook.GetComponent<Animator>().SetBool("Move_forward", true);
                    Press_Y.gameObject.SetActive(false);
                    StartCircle.gameObject.SetActive(false);
                    open_book = false;
                }

                
            }


        }
    }
}
