using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 檢測在場剩下多少elf，並進行特定事件
// 綁在 First/Second/Third Part Elf Object 上
public class CheckElfNumber : MonoBehaviour
{
    [SerializeField] FirstElfData_SO elfData;
    public static bool clear_all_Elf = false;

    [SerializeField] public GameObject Upside_UI;
    [SerializeField] GameObject MagicBook;
    [SerializeField] GameObject All_Little_Elf_PrefabParent;
    [SerializeField] public GameObject Directional_Light;
    [SerializeField] GameObject EndPartText;
    public AudioManager audioManager;
    private bool time_set = false;
    private int endState = 0;


    // 只是為了不讓if內的事件在Update()中重複觸發
    private bool hasBeenInvoked;
    private bool EndGame = false;
    void Update()
    {
        // 當前的elf都被打倒了
        // 每個事件觸發一次即可
        if (EventManager.stage==0 || EventManager.stage==1 || EventManager.stage == 2)
        {
            if(transform.childCount==0 && !hasBeenInvoked)
            {
                // // 打完 站樁怪 和 第一階段的兩隻
                // if(elfData.index == 3){
                //     EventManager.CreateElf(3);
                //     hasBeenInvoked = true;
                // }
                // 打完第二階段的三隻
                if(elfData.index == 4 )
                {
                    // 戰鬥結束，回到故事模式
                    EventManager.SwitchPlayMode("Combat_End");
                    clear_all_Elf = true;
                    hasBeenInvoked = true;
                }
            }
            // 場上還有elf
            else if(transform.childCount!=0)
            {
                hasBeenInvoked = false;
                clear_all_Elf = false;
            }

        }
        //王關!!!
        if (EventManager.stage == 3)
        { 
             if(BossElfController.BossMode == "End" && EndGame == false)
             {   
                
                     
                    EventManager.SwitchPlayMode("Combat_End");
                    audioManager.BossBattleStop();
                    audioManager.Ending();
                    for (int i = 0; i < Upside_UI.transform.childCount; i++)
                    {
                        //UI先全關掉
                        Upside_UI.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    
                    MagicBook.GetComponent<UpdatePlayerPosition>().enabled = true;
                    EventManager.StartDialogue(6);
                    
                    for (int i = 0; i < All_Little_Elf_PrefabParent.transform.childCount; i++)
                    {
                        //場上精靈都掛掉
                        
                        if (All_Little_Elf_PrefabParent.transform.GetChild(i).GetComponent<FirstElfController>() != null)
                        {
                            All_Little_Elf_PrefabParent.transform.GetChild(i).GetComponent<FirstElfController>().state = ElfStates.DEAD;
                        }
                        else if (All_Little_Elf_PrefabParent.transform.GetChild(i).GetComponent<SecondElfController>() != null)
                        {
                            All_Little_Elf_PrefabParent.transform.GetChild(i).GetComponent<SecondElfController>().state = ElfStates.DEAD;
                        }
                        else if (All_Little_Elf_PrefabParent.transform.GetChild(i).GetComponent<ThirdElfController>() != null)
                        {
                            All_Little_Elf_PrefabParent.transform.GetChild(i).GetComponent<ThirdElfController>().state = ElfStates.DEAD;
                        }
                        All_Little_Elf_PrefabParent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                        
                    endState = 1;
                    EndGame = true;
                       
                    
             } 
            
             if(endState == 1)
             {
                Invoke("Time_Set", 5.0f);
                if (time_set == true)
                {
                    
                    EndPartText.gameObject.SetActive(true);
                    EndPartText.transform.GetChild(0).GetComponent<UpdatePlayerPosition>().enabled = true;
                    Directional_Light.gameObject.SetActive(false);
                    MagicBook.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Close", true);
                    time_set = false;
                    CancelInvoke("Time_Set");
                    endState = 2;

                }
             }
             else if(endState == 2)
             {
                Invoke("Time_Set", 1.0f);
                if (time_set == true)
                {
                    
                    EventManager.StartDialogue(0);
                    time_set = false;
                    CancelInvoke("Time_Set");
                    endState = 3;

                }
             }
             else if(endState == 3)
             {
                Invoke("Time_Set", 3.0f);
                if (time_set == true)
                {
                    
                    EventManager.StartDialogue(1);
                    time_set = false;
                    CancelInvoke("Time_Set");
                    endState = 4;

                }
             }
             else if(endState == 4)
             {
                Invoke("Time_Set", 3.0f);
                if (time_set == true)
                {
                    
                    EventManager.StartDialogue(2);
                    time_set = false;
                    CancelInvoke("Time_Set");
                    endState = 5;

                }
             }
             else if(endState == 5)
             {
                Invoke("Time_Set", 3.0f);
                if (time_set == true)
                {
                    
                    EventManager.StartDialogue(3);
                    time_set = false;
                    CancelInvoke("Time_Set");
                    endState = 6;

                }
             }
             else if(endState == 6)
             {
                Invoke("Time_Set", 3.0f);
                if (time_set == true)
                {
                    
                    EventManager.StartDialogue(4);
                    time_set = false;
                    CancelInvoke("Time_Set");
                    endState = 7;

                }
             }
             else if(endState == 7)
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
