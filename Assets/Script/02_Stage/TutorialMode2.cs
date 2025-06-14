using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 綁在Energy Manager Object 上
public class TutorialMode2 : MonoBehaviour
{
    [SerializeField] OVRInput.Button LocatingButton;
    [SerializeField] private TextMeshProUGUI secondsText;
    [SerializeField] private AttackRange range;
    [SerializeField] GameObject AttackRange;
    [SerializeField] private Awaken awake;
    [SerializeField] private FireBreath fireData;
    [SerializeField] GameObject mouth;
    [SerializeField] public GameObject Upside_UI;
    [SerializeField] public GameObject Leftside_UI;
    [SerializeField] GameObject Little_DialogueUI;
    [SerializeField] GameObject Position_DialogueUI;
    public GameObject StartCircle;

    // 因為要偵測時停生效
    [SerializeField] private BoxBreath boxData;
    [SerializeField] private GameObject Arrow;
    [SerializeField] private GameObject energyBar;
    // 裝 Second Part Elf Object
    [SerializeField] Transform elfGroup;
    [SerializeField] GameObject MagicBook;
    [SerializeField] OVRInput.Button SummonMagicBookButton;
    private float locatingSeconds=5;

    private bool time_set = false;

    // 只是為了讓if中的事件，不會在Update()中被一直重複觸發
    private int tutorialState;
    private int breathe_sort = 0;
    //public AudioManager02 audioManager;

    private void Start() {

        MagicBook.gameObject.SetActive(false);
        EventManager.SwitchPlayMode("Initial");
        EventManager.stage = 1;
        //StartCircle.gameObject.SetActive(true);
        
    }

    private void Update() {

        // 睜眼效果結束
        if(awake.Awaked)
        {
            awake.Awaked = false;
            tutorialState = 1;
            EventManager.StartDialogue(0);
            //audioManager.PlayStory();
            
        }


        if (tutorialState == 1)
        {
            //等卷軸開完 才出現第一句章節介紹
            if (Little_DialogueUI.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Open_finished"))
            {
                EventManager.StartDialogue(0);
                //StartCircle.gameObject.SetActive(false);
                tutorialState = 2;
            }
            
        }
        else if (tutorialState == 2)
        {
            if (DialogueUI.currentState == 1)
            {
                //等卷軸到門前 才會自動切換技能成箱式呼吸、出現各種呼吸吐氣UI
                
                Invoke("Time_Set", 3.0f);
                if (time_set == true)
                {
                    ChangeSkill.changeskill = true;
                    for (int i = 0; i < Upside_UI.transform.childCount; i++)
                    {
                        Upside_UI.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    tutorialState = 3;
                    
                }
            }
        }

        else if (tutorialState == 3)
        {
            ChangeSkill.changeskill = false;
            tutorialState = 4;
        }
        else if (tutorialState == 4)
        {
            // 提示吸氣
            Upside_UI.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
            tutorialState = 5;
        }
        else if (tutorialState == 5)
        {
            // 吸氣完成
            if (BoxBreath.holding)
            {
                Upside_UI.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                tutorialState = 6;
            }
        }
        else if (tutorialState == 6)
        {
            // 提示閉氣
            Upside_UI.transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
            tutorialState = 7;

        }
        else if (tutorialState == 7)
        {
            // 閉氣完成
            if (BoxBreath.exhaling)
            {
                Upside_UI.transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                tutorialState = 8;
            }
        }
        else if (tutorialState == 8)
        {
            // 提示吐氣
            Upside_UI.transform.GetChild(2).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
            tutorialState = 9;

        }
        else if (tutorialState == 9)
        {
            // 吐氣完成 時停生效
            if (BoxBreath.holding2)
            {
                Upside_UI.transform.GetChild(2).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                tutorialState = 10;
            }
        }

        else if (tutorialState == 10)
        {
            // 提示閉氣
            Upside_UI.transform.GetChild(3).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
            tutorialState = 11;
        }
        else if (tutorialState == 11)
        {
            // 二段閉氣完成
            if (BoxBreath.CoolDown)
            {
                Upside_UI.transform.GetChild(3).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                tutorialState = 12;
            }
        }

        else if (tutorialState == 12)
        {

            //關閉UpsideUI
            for (int i = 0; i < Upside_UI.transform.childCount; i++)
            {
                Upside_UI.transform.GetChild(i).gameObject.SetActive(false);
            }
            EventManager.StartDialogue(2);
            tutorialState = 13;
        }
        else if (tutorialState == 13)
        {
            Invoke("Time_Set", 4f);
            if (time_set == true)
            {
                //召喚站樁怪
                EventManager.CreateElf(1);
                // 只有x座標有用處
                //EventManager.TutoralElfMove(new Vector3(-0.85f, 0.177f, 0.17f));
                tutorialState = 14;
                time_set = false;
                CancelInvoke("Time_Set");

            }
            
                
        }
        else if (tutorialState == 14)
        {
            
            //開啟UpsideUI
            for (int i = 0; i < Upside_UI.transform.childCount; i++)
            {
                Upside_UI.transform.GetChild(i).gameObject.SetActive(true);
            }

            EventManager.StartDialogue(3);
            tutorialState = 15;
                
            
        }

        else if (tutorialState == 15)
        {
            //框住站樁怪!
            
            if (BoxBreath.inhaling)
            {
                // 提示吸氣
                Debug.Log("提示吸氣");
                Upside_UI.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
                EventManager.StartDialogue(4);
            }  

            // 吸氣完成，提示閉氣
            if (BoxBreath.holding && !BoxBreath.exhaling )
            {
                Debug.Log("吸氣完成，提示閉氣");
                Debug.Log(BoxBreath.exhaling);
                Upside_UI.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                Upside_UI.transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
                EventManager.StartDialogue(5);
            }  

            // 閉氣完成，提示吐氣
            if (BoxBreath.exhaling)
            {
                Debug.Log("閉氣完成，提示吐氣");
                Upside_UI.transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                Upside_UI.transform.GetChild(2).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
                
            }
            

            // 吐氣完成，提示閉氣
            if (BoxBreath.holding2)
            {
                Debug.Log("吐氣完成，提示閉氣");
                Upside_UI.transform.GetChild(2).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                Upside_UI.transform.GetChild(3).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
                EventManager.StartDialogue(6);
                
            }
            
            // 二段閉氣完成
            if (BoxBreath.CoolDown)
            {
                Upside_UI.transform.GetChild(3).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                EventManager.StartDialogue(7);
                
                tutorialState = 16;
            }
            
        }
        else if (tutorialState == 16)
        {
            Leftside_UI.transform.GetChild(2).gameObject.SetActive(true);
            if (ChangeSkill.currentSkill == 0)
            {
                //關閉UpsideUI
                for (int i = 0; i < Upside_UI.transform.childCount; i++)
                {
                    Upside_UI.transform.GetChild(i).gameObject.SetActive(false);
                }
                Leftside_UI.transform.GetChild(2).gameObject.SetActive(false);
                tutorialState = 17;
            }
        }
        else if (tutorialState == 17)
        {
            //打倒站樁怪
            if (elfGroup.transform.childCount == 0)
            {
                EventManager.StartDialogue(8);
                tutorialState = 18;
            }

        }
        else if (tutorialState == 18)
        {
            if(ChangeSkill.currentSkill == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    
                    Upside_UI.transform.GetChild(i).gameObject.SetActive(false);
                }

            }
            if(ChangeSkill.currentSkill == 1 )
            {
                for (int i = 0; i < 4; i++)
                {
                    
                    Upside_UI.transform.GetChild(i).gameObject.SetActive(true);
                }
                if (BoxBreath.inhaling)
                {
                    // 提示吸氣
                    Debug.Log("提示吸氣");
                    Upside_UI.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
                }  

                // 吸氣完成，提示閉氣
                if (BoxBreath.holding && !BoxBreath.exhaling )
                {
                    Debug.Log("吸氣完成，提示閉氣");
                    Debug.Log(BoxBreath.exhaling);
                    Upside_UI.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                    Upside_UI.transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
                }  

                // 閉氣完成，提示吐氣
                if (BoxBreath.exhaling)
                {
                    Debug.Log("閉氣完成，提示吐氣");
                    Upside_UI.transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                    Upside_UI.transform.GetChild(2).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
                    
                }
                

                // 吐氣完成，提示閉氣
                if (BoxBreath.holding2)
                {
                    Debug.Log("吐氣完成，提示閉氣");
                    Upside_UI.transform.GetChild(2).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                    Upside_UI.transform.GetChild(3).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
                    
                }
                
                // 二段閉氣完成
                if (BoxBreath.CoolDown)
                {
                    Upside_UI.transform.GetChild(3).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                    for (int i = 0; i < 4; i++)
                    {
                        
                        Upside_UI.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    
                }
                
            }
            if (EventManager.currentMode == "Combat_End")
            {
                
                MagicBook.GetComponent<UpdatePlayerPosition>().enabled = true;
                this.enabled = false;
            }
        }


            
        


        // // Combat mode就不需要這個component了
        // if (EventManager.currentMode == "Combat")
        // {
        //     if(!MagicBook.activeSelf)
        //     {
        //         MagicBook.GetComponent<UpdatePlayerPosition>().enabled = true;
        //         this.enabled = false;
        //     }
        // }
    }
    void Time_Set()
    {
        if (time_set == false)
        {
            time_set = true;
        }

    }
}
