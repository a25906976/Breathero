using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 綁在Energy Manager Object 上
public class BossStageMode : MonoBehaviour
{

    [SerializeField] private AttackRange range;
    [SerializeField] private Awaken awake;
    [SerializeField] private GameObject energyBar;
    [SerializeField] GameObject MagicBook;
    [SerializeField] GameObject mouth;
    [SerializeField] GameObject All_Little_Elf_PrefabParent;
    [SerializeField] public GameObject Directional_Light;
    [SerializeField] public GameObject Spot_Light;
    
    
    //Breathe skills

    //Boss
    [SerializeField] GameObject Boss;
    [SerializeField] List<Material> BossMats;
    //UI
    [SerializeField] GameObject Little_DialogueUI;
    [SerializeField] public GameObject Upside_UI;
    [SerializeField] public GameObject Leftside_UI;

    public AudioManager audioManager;
    private bool hasMusiced;
    
    private SkinnedMeshRenderer meshRenderer;
    private ChangeSkill changeskill;


    private float locatingSeconds=5;
    private int tutorialState;
    private bool time_set = false;
    public static bool killed_the_three_elf = false;

    // 避免每次Update()都進入Coroutine
    private bool on = true;

    private void Start() {

        Boss.SetActive(false);
        MagicBook.gameObject.SetActive(false);
        Directional_Light.gameObject.SetActive(false);
        Spot_Light.gameObject.SetActive(false);
        EventManager.SwitchPlayMode("Initial");
        EventManager.stage = 3;
        audioManager.BossOpenPlay();
    }

    private void Update() {
        
        
        AnimatorStateInfo animInfo = Boss.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if(on)
        {
            Debug.Log(awake.Awaked);
            if (awake.Awaked){
                
                MagicBook.SetActive(true);
                MagicBook.GetComponent<Animator>().SetBool("Move_forward", true);
                EventManager.open_book = false;
                EventManager.stage = 3;
                awake.Awaked = false;
                tutorialState = 0;

            }


            if (tutorialState == 0)
            {
                if (Little_DialogueUI.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Open_finished"))
                {
                    EventManager.StartDialogue(0);
                    tutorialState = 1;
                }

            }
            else if (tutorialState == 1 )
            {
                if(DialogueUI.next_sentance_Onclick == true)
                {
                    Invoke("Time_Set", 2f);
                    if (time_set == true)
                    { 
                        EventManager.StartDialogue(1);
                        DialogueUI.next_sentance_Onclick = false;
                        tutorialState = 2;
                        time_set = false;
                        CancelInvoke("Time_Set");

                    }
                }

            }
            else if (tutorialState == 2)
            { 
                Invoke("Time_Set", 1.5f);
                if (time_set == true)
                { 
                    //BOSS出現
                    Spot_Light.gameObject.SetActive(true);
                    Boss.SetActive(true);
                    EventManager.StartDialogue(2);
                    tutorialState = 3;
                    time_set = false;
                    CancelInvoke("Time_Set");
                }
                
            }

            else if (tutorialState == 3)
            {
                Invoke("Time_Set", 4.0f);
                if (time_set == true)
                {
                    //樹模式
                    if(!animInfo.IsName("ChangeType"))
                    {
                        Boss.gameObject.GetComponent<Animator>().SetBool("ChangeType", true);

                        EventManager.StartDialogue(3);
                        CancelInvoke("Time_Set");
                    }

                    if(animInfo.IsName("ChangeType"))
                    { 
                        Debug.Log("Treee!!");
                        if(animInfo.normalizedTime>=0.1f && hasMusiced == false)
                        { 
                            audioManager.BossChange();
                            hasMusiced = true;
                        }
                        if(animInfo.normalizedTime>=1.0f)
                        { 
                            tutorialState = 4;
                            time_set = false;
                            Boss.gameObject.GetComponent<Animator>().SetBool("Waving", true);
                            hasMusiced = false;
                        }
                        else if(animInfo.normalizedTime>=0.5f)
                        {
                            
                            Debug.Log("AfterAnimation!!");
                            meshRenderer = Boss.GetComponentInChildren<SkinnedMeshRenderer>();
                            BossMats = Boss.GetComponent<BossElfController>().BossMats;
                            Material[] TmpMats = meshRenderer.materials;
                            TmpMats[3] = BossMats[2];
                            meshRenderer.materials = TmpMats;
                        }
                    }
                }
            }
            else if (tutorialState == 4)
            {
                Invoke("Time_Set", 4.0f);
                if (time_set == true)
                { 
                    //岩石模式
                    if(!animInfo.IsName("ChangeType"))
                    {
                        Boss.gameObject.GetComponent<Animator>().SetBool("ChangeType", true);
                        EventManager.StartDialogue(4);
                        CancelInvoke("Time_Set");
                    }

                    if(animInfo.IsName("ChangeType"))
                    { 
                        if(animInfo.normalizedTime>=0.1f && hasMusiced == false)
                        { 
                            audioManager.BossChange();
                            hasMusiced = true;
                        }
                        if(animInfo.normalizedTime>=1.0f)
                        {
                            hasMusiced = false;
                            tutorialState = 5;
                            time_set = false;
                            Boss.gameObject.GetComponent<Animator>().SetBool("Waving", true);
                        }
                        else if(animInfo.normalizedTime>=0.5f)
                        {
                            
                            Debug.Log("Stoneeee!!");
                            meshRenderer = Boss.GetComponentInChildren<SkinnedMeshRenderer>();
                            BossMats = Boss.GetComponent<BossElfController>().BossMats;
                            Material[] TmpMats = meshRenderer.materials;
                            TmpMats[3] = BossMats[3];
                            TmpMats[1] = BossMats[4];
                            meshRenderer.materials = TmpMats;
                        }
                    }
                }

            }
            else if (tutorialState == 5)
            {
                Invoke("Time_Set", 4f);
                if (time_set == true)
                { 
                    //火模式
                    if(!animInfo.IsName("ChangeType"))
                    {
                        Boss.gameObject.GetComponent<Animator>().SetBool("ChangeType", true);
                        EventManager.StartDialogue(5);
                        CancelInvoke("Time_Set");
                    }
                    if(animInfo.IsName("ChangeType"))
                    { 
                        if(animInfo.normalizedTime>=0.1f && hasMusiced == false)
                        { 
                            audioManager.BossChange();
                            hasMusiced =true;
                        }
                        if(animInfo.normalizedTime>=1.0f)
                        {
                            hasMusiced =false;
                            tutorialState = 6;
                            time_set = false;
                            Boss.gameObject.GetComponent<Animator>().SetBool("Waving", true);
                        }
                        else if(animInfo.normalizedTime>=0.5f)
                        {
                            
                            Debug.Log("Fire!!");
                            meshRenderer = Boss.GetComponentInChildren<SkinnedMeshRenderer>();
                            BossMats = Boss.GetComponent<BossElfController>().BossMats;
                            Material[] TmpMats = meshRenderer.materials;
                            TmpMats[1] = BossMats[5];
                            TmpMats[2] = BossMats[0];
                            meshRenderer.materials = TmpMats;
                        }
                    }
                }


            }
            else if (tutorialState == 6)
            { 
            
                Invoke("Time_Set", 6.5f);
                if (time_set == true)
                { 
                    //跳下來!!!開始戰鬥！
                    Boss.GetComponent<Animator>().SetBool("JumpDown", true);
                    Spot_Light.gameObject.SetActive(false);
                    Directional_Light.gameObject.SetActive(true);
                    audioManager.BossOpenStop();
                    audioManager.BossBattlePlay();
                    Debug.Log("戰鬥!");
                    tutorialState = 7;
                    time_set = false;
                    CancelInvoke("Time_Set");
                }
                
                
            }
            else if (tutorialState == 7)
            { 
                EventManager.SwitchPlayMode("Combat");
                energyBar.gameObject.SetActive(true);
                mouth.gameObject.SetActive(true);
                tutorialState = 8;
            }
            else if (tutorialState == 8)
            { 
                //UPside提示UI專區
                // Debug.Log("888");
                //火呼吸
                if (ChangeSkill.currentSkill == 0)
                { 
                    // Debug.Log("火呼吸!");
                    for (int i = 2; i < Upside_UI.transform.childCount; i++)
                    {
                        //先全關掉
                        Upside_UI.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    Upside_UI.transform.GetChild(0).gameObject.SetActive(true);
                    Upside_UI.transform.GetChild(1).gameObject.SetActive(true);

                    // 可吸氣
                    if (FireBreath.inhaling == true)
                    {
                        Upside_UI.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
                        Upside_UI.transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                    }
                    // 可吐氣
                    if (FireBreath.Inhaling_finish == true)
                    {
                        Upside_UI.transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
                        Upside_UI.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Cool", true);

                    }                


                }

                //箱式呼吸
                if (ChangeSkill.currentSkill == 1)
                { 
                    Debug.Log("箱式呼吸!");
                    Upside_UI.transform.GetChild(0).gameObject.SetActive(false);
                    Upside_UI.transform.GetChild(1).gameObject.SetActive(false);
                    for (int i = 2; i < 6; i++)
                    {
                        
                        Upside_UI.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    Upside_UI.transform.GetChild(6).gameObject.SetActive(false);
                    Upside_UI.transform.GetChild(7).gameObject.SetActive(false);

                    // 提示吸氣
                    if (BoxBreath.inhaling)
                    {
                        Upside_UI.transform.GetChild(2).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
                    }  

                    // 吸氣完成，提示閉氣
                    if (BoxBreath.holding && !BoxBreath.exhaling )
                    {
                        Upside_UI.transform.GetChild(2).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                        Upside_UI.transform.GetChild(3).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
                    }  

                    // 閉氣完成，提示吐氣
                    if (BoxBreath.exhaling)
                    {
                        Upside_UI.transform.GetChild(3).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                        Upside_UI.transform.GetChild(4).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
                
                    }
            
                    // 吐氣完成，提示閉氣
                    if (BoxBreath.holding2)
                    {
                        Upside_UI.transform.GetChild(4).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                        Upside_UI.transform.GetChild(5).gameObject.GetComponent<Animator>().SetBool("Flashing", true);
                
                    }
            
                    // 二段閉氣完成
                    if (BoxBreath.CoolDown)
                    {
                        Upside_UI.transform.GetChild(3).gameObject.GetComponent<Animator>().SetBool("Cool", true);
                    }             


                }

                //完全呼吸
                if (ChangeSkill.currentSkill == 2)
                { 
                    Debug.Log("完全呼吸!");
                    for (int i = 0; i < Upside_UI.transform.childCount; i++)
                    {
                        //先全關掉
                        Upside_UI.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    Upside_UI.transform.GetChild(7).gameObject.SetActive(false);
                    Upside_UI.transform.GetChild(6).gameObject.SetActive(true);
                    //吸氣完成，提示吐氣
                    if (FullBreath.exhaling == true)
                    {
                        Upside_UI.transform.GetChild(6).gameObject.SetActive(false);
                        Upside_UI.transform.GetChild(7).gameObject.SetActive(true);

                    }
                    //吐氣完成，關UI
                    if (FullBreath.CoolDown == true)
                    {
                        Upside_UI.transform.GetChild(7).gameObject.SetActive(false);
                    }


                }
                if(BossElfController.BossMode == "End")
                { 
                    for (int i = 0; i < Upside_UI.transform.childCount; i++)
                    {
                        //先全關掉
                        Upside_UI.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    this.enabled = false;
                    tutorialState = 9;
                } 
                
            }
            else if (tutorialState == 9)
            {
                
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
