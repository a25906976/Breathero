using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 綁在Energy Manager Object 上
public class TutorialMode : MonoBehaviour
{
    [SerializeField] OVRInput.Button LocatingButton;
    // mouth開啟，所有魔法攻擊才能生效
    [SerializeField] GameObject mouth;
    // 定位秒數文字
    [SerializeField] private TextMeshProUGUI secondText;
    [SerializeField] private AttackRange range;
    [SerializeField] private FireBreath fire;
    [SerializeField] private GameObject Arrow;
    [SerializeField] private GameObject energyBar;
    [SerializeField] private GameObject MagicBook;
    // 裝 First Part Elf Object
    [SerializeField] Transform elfGroup;
    [SerializeField] public GameObject Fire_Breathe_icon;
    //[SerializeField] public GameObject Breathe_icon;
    [SerializeField] public GameObject Upside_UI;
    [SerializeField] public GameObject Leftside_UI;
    [SerializeField] GameObject Little_DialogueUI;
    [SerializeField] GameObject Position_DialogueUI;

    [SerializeField] OVRInput.Button SummonMagicBookButton;

    private bool after_exhaling = false;

    private bool time_set = false;
    private float locatingSeconds=5;

    // 只是為了讓if中的事件，不會在Update()中被一直重複觸發
    private int tutorialState = 0;


    //private void Start()
    //{
    //    if (EnterStartCircle.enter_start_circle == true)
    //    {
    //        EventManager.SwitchPlayMode("Initial");
    //    }

    //}
    private void Start()
    {
        EventManager.stage = 0;
    }
    private void Update() {


        //進入start_circle 開始第一關Intial mode
        if (EnterStartCircle.enter_start_circle == true && tutorialState == 0)
        {

            EventManager.SwitchPlayMode("Initial");
            Upside_UI.transform.GetChild(0).gameObject.SetActive(false);
            tutorialState = 1;

        }
        else if (tutorialState == 1)
        {

            //等書的動畫結束 才會出現第一段字
            if (Little_DialogueUI.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Open_finished"))
            {
                EventManager.StartDialogue(0);
                tutorialState = 2;
            }

        }
        else if (tutorialState == 2)
        {
            if (DialogueUI.currentState == 3)
            {
                //等卷軸到門前 才會出現各種呼吸吐氣UI
                Invoke("Time_Set", 3.0f);
                if (time_set == true)
                {
                    // 可吸氣
                    if (FireBreath.inhaling == true)
                    {
                        Upside_UI.transform.GetChild(1).gameObject.SetActive(true);
                        Leftside_UI.transform.GetChild(0).gameObject.SetActive(true);
                        Upside_UI.transform.GetChild(2).gameObject.SetActive(false);
                        Leftside_UI.transform.GetChild(1).gameObject.SetActive(false);

                    }
                    // 可吐氣
                    if (FireBreath.Inhaling_finish == true)
                    {
                        Upside_UI.transform.GetChild(2).gameObject.SetActive(true);
                        Leftside_UI.transform.GetChild(1).gameObject.SetActive(true);
                        Upside_UI.transform.GetChild(1).gameObject.SetActive(false);
                        Leftside_UI.transform.GetChild(0).gameObject.SetActive(false);

                    }
                    if (FireBreath.Exhaling_finish == true)
                    {
                        tutorialState = 3;
                    }

                }

            }


        }

        else if (tutorialState == 3)
        {
            Upside_UI.transform.GetChild(2).gameObject.SetActive(false);
            Leftside_UI.transform.GetChild(1).gameObject.SetActive(false);
            EventManager.StartDialogue(4);
            tutorialState = 4;
        }

        else if (tutorialState == 4)
        {
            Invoke("Time_Set", 4.5f);
            if (time_set == true)
            {
                //召喚站樁怪
                EventManager.CreateElf(1);
                time_set = false;
                tutorialState = 5;
                CancelInvoke("Time_Set");
            }
        }

        else if (tutorialState == 5)
        {
            
            EventManager.StartDialogue(5);
            tutorialState = 6;
                
        }
        else if (tutorialState == 6)
        {
            // 可吸氣
            if (FireBreath.inhaling == true)
            {
                Upside_UI.transform.GetChild(1).gameObject.SetActive(true);
                Upside_UI.transform.GetChild(2).gameObject.SetActive(false);
            }
            // 可吐氣
            if (FireBreath.Inhaling_finish == true)
            {
                Upside_UI.transform.GetChild(1).gameObject.SetActive(true);
                Upside_UI.transform.GetChild(2).gameObject.SetActive(false);

            }
            if (elfGroup.childCount == 0)
            {
                Debug.Log("Killed the tutorial ELF!!!!!!!!!");
                Upside_UI.transform.GetChild(1).gameObject.SetActive(false);
                Upside_UI.transform.GetChild(2).gameObject.SetActive(false);
                tutorialState = 7;
            }
        }
        else if (tutorialState == 7)
        {
            EventManager.StartDialogue(6);
            tutorialState = 8;

        }
        else if (tutorialState == 8)
        { 
            if (CheckElfNumber.clear_all_Elf == true)
            {
                EventManager.StartDialogue(7);
                EventManager.OpenDoor();
               
                tutorialState = 9;
            }
               
        }
        else if (tutorialState == 9)
        {
            Invoke("Time_Set", 8f);
            if (time_set == true)
            { 
                MagicBook.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Close",true);
                time_set = false;
                tutorialState = 10;
                CancelInvoke("Time_Set");
            }
             
        }
        else if (tutorialState == 10)
        {
            Debug.Log("Finish Fisrt Stage!!");
        }

            // 站樁怪進攻擊範圍

            //if (range.targetInRange){
            //if(tutorialState==0){

                //EventManager.StartDialogue(5);
                //Fire_Breathe_icon.gameObject.SetActive(true);
                //EventManager.DialogueState = 8;
                //tutorialState = 3;
            //}
            //else if(tutorialState==1){
            //    if(OVRInput.Get(LocatingButton, OVRInput.Controller.LTouch))
            //    {
            //        secondText.gameObject.SetActive(true);
            //        tutorialState = 2;
            //    }
            //}
            //else if(tutorialState == 2)
            //{
            //    EventManager.StartPrompt(4);
            //    if(OVRInput.GetUp(LocatingButton, OVRInput.Controller.LTouch)){
            //        locatingSeconds = 5f;
            //    }
            //    if(OVRInput.Get(LocatingButton, OVRInput.Controller.LTouch))
            //    {
            //        locatingSeconds -= Time.deltaTime;
            //        // 定位完成
            //        if(locatingSeconds < 0)
            //        {
            //            secondText.gameObject.SetActive(false);
            //            tutorialState = 3;
            //        }
            //    }
            //    // 顯示定位秒數
            //    secondText.text = Mathf.Ceil(locatingSeconds) + " s";
            //}
            //else if(tutorialState == 3)
            //{
            //    //secondText.gameObject.SetActive(false);
            //    //EventManager.StartPrompt(12);
            //    EventManager.StartDialogue(8);
            //    //Breathe_icon.gameObject.SetActive(true);
            //    EventManager.DialogueState = 9;
                
                 
            //    // 呼吸教學箭頭
            //    Arrow.SetActive(true);
            //    // 能量條
            //    energyBar.SetActive(true);
            //    // 要能施法
            //    mouth.SetActive(true);
            //    tutorialState = 4;
                
            //}
            //else if(tutorialState == 4)
            //{   
            //    // 打完站樁怪
            //    if(elfGroup.GetComponentInChildren<FirstElfController>().health==0)
            //    {
            //        // 箭頭消失
            //        Fire_Breathe_icon.gameObject.SetActive(false);
            //        //Breathe_icon.gameObject.SetActive(false);
            //        Arrow.SetActive(false);
            //        EventManager.StartDialogue(10);
            //        EventManager.DialogueState = 11;
            //        tutorialState = 5;
            //    }
            //    // 可吐氣
            //    else if(fire.energySlider.value>=2)
            //    {
            //        EventManager.StartDialogue(9);
            //        EventManager.DialogueState = 10;
            //    }
            //    // 可吸氣
            //    else if(fire.energySlider.value<=0){
            //        EventManager.StartDialogue(8);
            //        EventManager.DialogueState = 9;
            //    }
            //}
        //}
        





        // Combat mode就不需要這個component了
        // 是在對話後切換到Combat mode
        if(EventManager.currentMode == "Combat")
        {
            if(!MagicBook.activeSelf)
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
