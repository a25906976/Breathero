using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 第零關
// 綁在 EventManager Object上
public class FirstStagePartZeroTutorial : MonoBehaviour
{
    // MagicBook都是指PositionForMagicBook (因為其出現位置，會根據Player的位置)
    [SerializeField] GameObject MagicBook;
    // 高台下方的白色圈
    [SerializeField] GameObject StartCircle;
    [SerializeField] private Awaken awake;
    public GameObject Double_hand_controll;
    [SerializeField] public GameObject Upside_UI;
    [SerializeField] public GameObject Directional_Light;
    [SerializeField] public GameObject Eventmanager;
    [SerializeField] public GameObject ZeroPartText;

    private bool Leftcontrol_trigger = false;
    private bool Rightcontrol_trigger = false;
    private bool time_set = false;

    public AudioManager audioManager;

    // 只是為了讓if中的事件，不會在Update()中被一直重複觸發
    private int tutorialState;
    
    void Start()
    {
        
        //指定關卡
        EventManager.stage = 0;

        //第零關開始時，還沒有魔法書
        MagicBook.gameObject.SetActive(false);
        //MagicBook.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Close", true);
        //第零關從None模式開始
        SwitchMode.Mode_Content = "None";
        EventManager.SwitchPlayMode("None");
        //提示要左右手controller移動
        
        Directional_Light.gameObject.SetActive(false);
        //audioManager.PlatStory();
    }

    // MagicBook的行為
    void Update()
    {
        if (awake.Awaked)
        {
            awake.Awaked = false;
            tutorialState = 1;
            
        }

        else if (tutorialState == 1 )
        { 
            Invoke("Time_Set", 1.0f);
            if (time_set == true)
            { 
                EventManager.StartDialogue(0);
                time_set = false;
                tutorialState = 2;
                CancelInvoke("Time_Set");
            }
        }
        else if (tutorialState == 2 )
        { 
            Invoke("Time_Set", 4.0f);
            if (time_set == true)
            { 
                EventManager.StartDialogue(1);
                time_set = false;
                tutorialState = 3;
                CancelInvoke("Time_Set");
            }
        }
        else if (tutorialState == 3 )
        { 
            Invoke("Time_Set", 4.0f);
            if (time_set == true)
            { 
                EventManager.StartDialogue(2);
                time_set = false;
                tutorialState = 4;
                CancelInvoke("Time_Set");
            }
        }
        else if (tutorialState == 4 )
        { 
            Invoke("Time_Set", 4.0f);
            if (time_set == true)
            { 
                EventManager.StartDialogue(3);
                time_set = false;
                tutorialState = 5;
                CancelInvoke("Time_Set");
            }
        }
        else if (tutorialState == 5 )
        { 
            Invoke("Time_Set", 4.0f);
            if (time_set == true)
            { 
                EventManager.StartDialogue(4);
                time_set = false;
                tutorialState = 6;
                CancelInvoke("Time_Set");
            }
        }
        else if (tutorialState == 6 )
        { 
            Invoke("Time_Set", 4.0f);
            if (time_set == true)
            {
                Double_hand_controll.gameObject.SetActive(true);
                Double_hand_controll.GetComponent<Animator>().SetBool("Fade_IN_bool", true);
                Directional_Light.gameObject.SetActive(true);
                ZeroPartText.gameObject.SetActive(false);
                time_set = false;
                tutorialState = 7;
                CancelInvoke("Time_Set");

            }
        }

        else if (tutorialState == 7 )
        {
            //兩手controller移動、轉視角提示 跟判定
            //Double_hand_controll.GetComponent<Animator>().SetBool("Start_flashing_bool", true);
            if ((OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp, OVRInput.Controller.LTouch) ||
                 OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown, OVRInput.Controller.LTouch) ||
                 OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.LTouch) ||
                 OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.LTouch)))
            {
                Leftcontrol_trigger = true;
            }
            if ((OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp, OVRInput.Controller.RTouch) ||
                 OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown, OVRInput.Controller.RTouch)||
                 OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.RTouch)||
                 OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.RTouch)))
            {
                Rightcontrol_trigger = true;
            }

            else if (Leftcontrol_trigger == true && Rightcontrol_trigger == true)
            {
                tutorialState = 8;
                Double_hand_controll.GetComponent<Animator>().SetBool("Fade_OUT_bool", true);
                StartCircle.gameObject.SetActive(true);
                //StartCircle.GetComponent<Animator>().SetBool("Flashing", true);
                Upside_UI.transform.GetChild(0).gameObject.SetActive(true);
                Double_hand_controll.gameObject.SetActive(false);
            }
            

        }
        else if (tutorialState == 8)
        {
            Eventmanager.GetComponent<TutorialMode>().enabled = true;
            EventManager.StartDialogue(1);
            // part zero tutorial finish
            this.enabled = false;
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
