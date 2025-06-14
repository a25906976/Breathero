using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 綁在Energy Manager Object 上
public class TutorialMode3 : MonoBehaviour
{

    [SerializeField] FirstElfData_SO elfData;
    [SerializeField] OVRInput.Button LocatingButton;
    [SerializeField] private TextMeshProUGUI secondsText;
    [SerializeField] private AttackRange range;
    [SerializeField] private Awaken awake;
    [SerializeField] private FireBreath fireData;
    [SerializeField] private BoxBreath boxData;
    [SerializeField] private GameObject Arrow;
    [SerializeField] private GameObject energyBar;
    [SerializeField] Transform elfGroup;
    [SerializeField] GameObject MagicBook;
    [SerializeField] OVRInput.Button switchButton;
    [SerializeField] GameObject mouth;
    [SerializeField] public GameObject Upside_UI;
    [SerializeField] public GameObject Leftside_UI;
    [SerializeField] OVRInput.Button SummonMagicBookButton;
    [SerializeField] public GameObject Shield_container;
    [SerializeField] GameObject StartCircle;
    [SerializeField] GameObject Little_DialogueUI;
    [SerializeField] GameObject Position_DialogueUI;

    private float locatingSeconds = 5;
    private int tutorialState;
    private bool time_set = false;
    public static bool killed_the_three_elf = false;

    // 避免每次Update()都進入Coroutine
    private bool on = true;

    private void Start()
    {
        //// 一開始就顯示AttackRange (在火呼吸的時候?)
        //range.gameObject.SetActive(true);
        MagicBook.gameObject.SetActive(false);
        EventManager.SwitchPlayMode("Initial");
        EventManager.stage = 2;
        //StartCircle.gameObject.SetActive(true);

    }

    private void Update()
    {
        if (on)
        {

            if (awake.Awaked)
            {
                awake.Awaked = false;
                tutorialState = 0;
            }
            if (tutorialState == 0)
            {
                if (Little_DialogueUI.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Open_finished"))
                {
                    EventManager.StartDialogue(0);
                    //StartCircle.gameObject.SetActive(false);
                    tutorialState = 1;
                }
                
            }

            if (tutorialState == 1)
            {
                
                if (DialogueUI.currentState == 1)
                {
                    Invoke("Time_Set", 3.0f);
                    if (time_set == true)
                    {
                        energyBar.gameObject.SetActive(true);
                        ChangeSkill.changeskill = true;
                        Upside_UI.transform.GetChild(0).gameObject.SetActive(true);
                        Leftside_UI.transform.GetChild(3).gameObject.SetActive(true);
                        tutorialState = 2;
                        time_set = false;
                        CancelInvoke("Time_Set");
                    }
                }

            }
            else if (tutorialState == 2)
            {
                ChangeSkill.changeskill = false;
                tutorialState = 3;

            }
            //換成完全呼吸
            else if (tutorialState == 3)
            {
                Invoke("Time_Set", 1.5f);
                if (time_set == true)
                {
                    ChangeSkill.changeskill = true;
                    time_set = false;
                    CancelInvoke("Time_Set");
                    tutorialState = 4;
                }

            }
            else if (tutorialState == 4)
            {
                ChangeSkill.changeskill = false;
                tutorialState = 5;
            }
            else if (tutorialState == 5)
            {
                //吸氣完成，提示吐氣 手放下UI
                if (FullBreath.exhaling == true)
                {
                    Upside_UI.transform.GetChild(0).gameObject.SetActive(false);
                    Leftside_UI.transform.GetChild(3).gameObject.SetActive(false);
                    Upside_UI.transform.GetChild(1).gameObject.SetActive(true);
                    Leftside_UI.transform.GetChild(4).gameObject.SetActive(true);

                }
                //吐氣完成，關UI
                if (FullBreath.CoolDown == true)
                {
                    Upside_UI.transform.GetChild(1).gameObject.SetActive(false);
                    Leftside_UI.transform.GetChild(4).gameObject.SetActive(false);
                    tutorialState = 6;
                }

            }
            else if (tutorialState == 6)
            {
                EventManager.StartDialogue(2);
                Invoke("Time_Set", 3.5f);
                if (time_set == true)
                {
                    Object.Destroy(Shield_container.transform.GetChild(0).gameObject);
                    time_set = false;
                    tutorialState = 7;
                    CancelInvoke("Time_Set");
                }

            }
            else if (tutorialState == 7)
            {
                Invoke("Time_Set", 2f);
                if (time_set == true)
                {
                    // 站樁怪出現
                    EventManager.CreateElf(1);
                    range.gameObject.SetActive(false);
                    time_set = false;
                    tutorialState = 8;
                    CancelInvoke("Time_Set");
                }

            }
            else if (tutorialState == 8)
            {
                Invoke("Time_Set", 2f);
                if (time_set == true)
                {
                    EventManager.StartDialogue(3);
                    ThirdElfController.Pure_Charge_Time = 60f;
                    ThirdElfController.Stop_Charge = false;
                    elfGroup.GetChild(0).GetComponent<ThirdElfController>().state = ElfStates.PURE_CHARGE;
                    time_set = false;
                    tutorialState = 9;
                    CancelInvoke("Time_Set");
                }
            }
            else if (tutorialState == 9)
            {
                Invoke("Time_Set", 2f);
                if (time_set == true)
                {
                    EventManager.StartDialogue(4);
                    Upside_UI.transform.GetChild(0).gameObject.SetActive(true);
                    tutorialState = 10;
                }
            }
            else if (tutorialState == 10)
            {
                //吸氣完成
                if (FullBreath.exhaling == true)
                {
                    Upside_UI.transform.GetChild(0).gameObject.SetActive(false);
                    Upside_UI.transform.GetChild(1).gameObject.SetActive(true);

                }
                //吐氣完成，關UI 架完盾
                else if (FullBreath.CoolDown == true)
                {
                    Upside_UI.transform.GetChild(1).gameObject.SetActive(false);
                    tutorialState = 11;
                }

            }
            else if (tutorialState == 11)
            {
                //過兩秒開始噴火
                Invoke("Time_Set", 2f);
                if (time_set == true)
                {
                    ThirdElfController.Stop_Charge = true;
                    elfGroup.GetChild(0).GetComponent<ThirdElfController>().state = ElfStates.PURE_FIRE;
                    time_set = false;
                    tutorialState = 12;
                    CancelInvoke("Time_Set");
                }
            }
            else if (tutorialState == 12)
            {
                EventManager.StartDialogue(5);
                tutorialState = 13;

            }
            else if (tutorialState == 13)
            {
                if (elfGroup.GetComponentInChildren<ThirdElfController>().health == 0)
                {
                    EventManager.StartDialogue(6);
                    tutorialState = 14;
                }

            }
            else if (tutorialState == 14)
            { 
                
            }
                //if (tutorialState == 0)
                //{
                //    Invoke("Time_Set", 7.0f);
                //    if (time_set == true)
                //    {
                //        // 站樁怪出現
                //        EventManager.CreateElf(1);
                //        time_set = false;
                //        tutorialState = 1;
                //        CancelInvoke("Time_Set");
                //    }

                //}

                //else if (tutorialState == 1)
                //{
                //    Invoke("Time_Set", 4.0f);
                //    if (time_set == true)
                //    {
                //        //集氣2秒
                //        ThirdElfController.Pure_Charge_Time = 5.0f;
                //        ThirdElfController.Stop_Charge = false;
                //        elfGroup.GetChild(0).GetComponent<ThirdElfController>().state = ElfStates.PURE_CHARGE;
                //        EventManager.StartDialogue(1);
                //        EventManager.DialogueState = 2;
                //        time_set = false;
                //        tutorialState = 2;
                //        CancelInvoke("Time_Set");
                //    }
                //}
                //else if (tutorialState == 2)
                //{
                //    //Fire
                //    Invoke("Time_Set", 4.0f);
                //    if (time_set == true)
                //    {
                //        elfGroup.GetChild(0).GetComponent<ThirdElfController>().state = ElfStates.PURE_FIRE;
                //        time_set = false;
                //        tutorialState = 3;
                //        CancelInvoke("Time_Set");
                //    }

                //}
                //else if (tutorialState == 3)
                //{
                //    //死掉
                //    Invoke("Time_Set", 1.0f);
                //    if (time_set == true)
                //    {
                //        EventManager.Die(false);
                //        time_set = false;
                //        CancelInvoke("Time_Set");
                //        tutorialState = 4;
                //    }

                //}

                //else if (tutorialState == 4)
                //{

                //    //第一隻教學怪除掉
                //    Invoke("Time_Set", 3.5f);
                //    if (time_set == true)
                //    {
                //        Destroy(elfGroup.GetChild(0).gameObject);

                //        tutorialState = 5;
                //        time_set = false;
                //        CancelInvoke("Time_Set");
                //    }

                //}
                //else if (tutorialState == 5)
                //{
                //    //復活
                //    //魔法書回到正面
                //    EventManager.MagicBookAction("Left");
                //    EventManager.StartDialogue(2);
                //    Invoke("Time_Set", 3.0f);
                //    if (time_set == true)
                //    {
                //        EventManager.Die(true);
                //        time_set = false;
                //        tutorialState = 6;
                //        CancelInvoke("Time_Set");

                //    }
                //}

                //else if (tutorialState == 6)
                //{
                //    Invoke("Time_Set", 2.5f);
                //    if (time_set == true)
                //    {
                //        //召喚第二隻教學站樁怪
                //        EventManager.CreateElf(1);
                //        //elfGroup.GetChild(0).GetComponent<ThirdElfController>().state = ElfStates.IDLE;
                //        //elfGroup.GetChild(0).GetComponent<ThirdElfController>().state = ElfStates.WALK;
                //        //EventManager.TutoralElfMove(new Vector3(-26.04f, 0f, 0f));
                //        time_set = false;
                //        tutorialState = 7;
                //        CancelInvoke("Time_Set");
                //    }
                //}
                //else if (tutorialState == 7)
                //{
                //    Invoke("Time_Set", 2.0f);
                //    if (time_set == true)
                //    {

                //        time_set = false;
                //        tutorialState = 8;
                //        CancelInvoke("Time_Set");
                //    }
                //}

                //else if (tutorialState == 8)
                //{
                //    if (EventManager.DialogueState == 3)
                //    {
                //        ThirdElfController.Pure_Charge_Time = 60f;
                //        ThirdElfController.Stop_Charge = false;
                //        elfGroup.GetChild(0).GetComponent<ThirdElfController>().state = ElfStates.PURE_CHARGE;
                //        tutorialState = 9;
                //    }
                //}
                //else if (tutorialState == 9)
                //{
                //    EventManager.StartDialogue(5);
                //    mouth.SetActive(true);
                //    Arrow.SetActive(true);
                //    energyBar.SetActive(true);
                //    tutorialState = 10;
                //}
                //else if (tutorialState == 10)
                //{
                //    if (ChangeSkill.currentSkill == 2)
                //    {
                //        EventManager.StartDialogue(6);
                //        tutorialState = 11;
                //    }

                //}
                //else if (tutorialState == 11)
                //{
                //    if (FullBreath.ShieldEffect == true)
                //    {
                //        EventManager.StartDialogue(7);

                //        Invoke("Time_Set", 3.0f);
                //        //吐氣超過2秒 教學怪才會開始噴火 (Tutorial_Start_Fire 控制)
                //        if (time_set == true && FullBreath.Tutorial_Start_Fire == true)
                //        {
                //            ThirdElfController.Stop_Charge = true;
                //            elfGroup.GetChild(0).GetComponent<ThirdElfController>().state = ElfStates.PURE_FIRE;
                //            time_set = false;
                //            tutorialState = 12;
                //            CancelInvoke("Time_Set");
                //            FullBreath.Tutorial_Start_Fire = false;
                //        }

                //    }
                //}
                //else if (tutorialState == 12)
                //{
                //    //火精靈噴完火，user可以切換回火呼吸幹掉他ㄌ
                //    Invoke("Time_Set", 4.5f);
                //    if (time_set == true)
                //    {
                //        EventManager.StartDialogue(8);
                //        elfGroup.GetChild(0).GetComponent<ThirdElfController>().state = ElfStates.COOLDOWN;
                //        tutorialState = 13;
                //        CancelInvoke("Time_Set");
                //    }
                //}
                //else if (tutorialState == 13)
                //{
                //    if (elfGroup.GetComponentInChildren<ThirdElfController>().health == 0)
                //    {
                //        EventManager.StartDialogue(9);
                //        Arrow.SetActive(false);
                //        tutorialState = 14;
                //    }
                //}
                //else if (tutorialState == 14 && killed_the_three_elf == true && elfGroup.transform.childCount == 0)
                //{

                //    EventManager.StartDialogue(10);

                //}
            }

            // Combat mode就不需要這個component了
            if (EventManager.currentMode == "Combat")
            {
                if (!MagicBook.activeSelf)
                {
                    MagicBook.GetComponent<UpdatePlayerPosition>().enabled = true;
                    this.enabled = false;
                }
            }
            


     }

    void Time_Set()
    {
        if (time_set == false)
        {
            time_set = true;
        }

    }

}
    

