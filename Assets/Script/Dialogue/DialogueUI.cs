using System.Collections;
using System.Collections.Generic;
using TMPro;
using Dialogue;
using UnityEngine;
using UnityEngine.UI;

// 處理魔法書的對話內容
// 綁在 DialogueUI Object 上
public class DialogueUI : MonoBehaviour
{
    [SerializeField] Transform elfGroup;
    public ElfStates state;
    // 是TextMeshProUGUI 非TextMeshPro
    [SerializeField] private TextMeshProUGUI sentenceText;

    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button SkipButton;

    // MagicBook都是指PositionForMagicBook (因為其出現位置，會根據Player的位置)
    [SerializeField] private GameObject MagicBook;
    [SerializeField] public GameObject Upside_UI;
    [SerializeField] public GameObject Leftside_UI;

    public static bool next_sentance_Onclick = false;

    // 僅作用在該script內，判斷對話進展到哪個state
    public static int currentState;
    private bool dialogueEnd;

    private Queue<string> _sentences;

    private void Awake() {
        _sentences = new Queue<string>();
    }

    private void OnEnable() {
        EventManager.BookActionEvent += CloseBook;
    }
    private void OnDisable() {
        EventManager.BookActionEvent -= CloseBook;
    }

    // 如果有 關書的動畫 也許能放在這
    private void CloseBook(string _)
    {
        // TODO::
    }

    // 裝填句子
    public void SetSentences(IEnumerable<string> sentences, int state)
    {
        // // 因為之後要對話，所以文字、按鈕都要顯示
        // sentenceText.gameObject.SetActive(true);
        // ContinueButton.gameObject.SetActive(true);
        // SkipButton.gameObject.SetActive(true);
        dialogueEnd = false;
        currentState = state;
        Debug.Log("currentState :" + currentState);
        _sentences.Clear();
        foreach(var sentence in sentences)
        {
            _sentences.Enqueue(sentence);
        }
        
        sentenceText.text = _sentences.Dequeue();
    }

    public void nextSentence()
    {
        if(_sentences.Count > 0)
        {
            sentenceText.text = _sentences.Dequeue();
        }
        else if(_sentences.Count == 0)
        {
            // tree elf's stage
            if(EventManager.stage==0)
            {
                Debug.Log("Press!!");
                if (currentState == 0)
                {
                    EventManager.StartDialogue(1);
                }
                else if (currentState == 1)
                { 
                    EventManager.StartDialogue(2);
                }
                else if (currentState == 2)
                {
                    EventManager.StartDialogue(3);
                }
                else if (currentState == 3)
                {
                    EventManager.SwitchPlayMode("Tutorial");
                }
                else if (currentState == 6)
                {
                    // 切到攻擊模式 (要打三隻怪)
                    EventManager.SwitchPlayMode("Combat");
                    EventManager.CreateElf(3);
                    EventManager.StartDialogue(7);
                }
                else if (currentState == 7)
                {

                    EventManager.OpenDoor();
                    MagicBook.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Close", true);
                }

            }

            // stone elf's stage
            else if(EventManager.stage==1)
            {

                if (currentState == 0)
                {
                    EventManager.StartDialogue(1);
                }
                else if (currentState == 1)
                {
                    EventManager.SwitchPlayMode("Tutorial");
                }
                
                else if (currentState == 8)
                {
                    // 切到攻擊模式 (要打三隻怪)
                    EventManager.SwitchPlayMode("Combat");
                    EventManager.CreateElf(3);

                }
                //else if(currentState==7)
                //{
                //    EventManager.OpenDoor();
                //    MagicBook.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Close",true);
                //}
                
            }

            // fire elf's stage
            else if (EventManager.stage == 2)
            {
                
                if (currentState == 0)
                {
                    EventManager.StartDialogue(1);
                }
                else if (currentState == 1)
                {
                    EventManager.SwitchPlayMode("Tutorial");
                }
                else if (currentState == 2)
                {
                    EventManager.StartDialogue(3);
                }
                    
                else if (currentState == 6)
                {
                    // 切到攻擊模式 (要打三隻怪)
                    EventManager.SwitchPlayMode("Combat");
                    EventManager.CreateElf(3);
                    EventManager.StartDialogue(7);

                }

            }
            // Boss stage
            else if (EventManager.stage == 3)
            { 
                if (currentState == 0)
                {
                    next_sentance_Onclick = true;
                    MagicBook.GetComponent<Animator>().SetBool("Boss_move_top", true);
                    ContinueButton.gameObject.SetActive(false);
                }
                


            }
        }
    }

    public void skipSentence()
    {
        _sentences.Clear();
        if(_sentences.Count == 0)
        {
            // tree elf's stage
            if(EventManager.stage==0)
            {
                if(!dialogueEnd)
                    if(currentState==0){
                       MagicBook.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Close",true);
                        // TODO: 顯示左手Y鍵按壓動畫
                    }
                    else if(currentState==3)
                    {
                        sentenceText.text = "";
                        // 魔法書 從高台 飛到 白色圓圈 內
                        MagicBook.GetComponent<Animator>().SetBool("MoveToCircle",true);
                        GetComponent<OVRRaycaster>().enabled = false;
                    }
                    else if(currentState==4){
                        // 開始教學 (打站樁怪)
                        SwitchMode.Mode_Content = "Tutorial";
                        EventManager.SwitchPlayMode("Tutorial");
                        // 站樁怪移到攻擊範圍內
                        // 只有x座標有作用
                        EventManager.TutoralElfMove(new Vector3(20.38f, 0f, 0f));
                    }
                    else if(currentState==5){
                        EventManager.StartDialogue(6);
                        EventManager.DialogueState = 7;
                    }
                    else if(currentState==10)
                    {
                        // 切到攻擊模式 (要打三隻怪)
                        SwitchMode.Mode_Content = "Combat";
                        EventManager.SwitchPlayMode("Combat");
                        EventManager.CreateElf(3);
                    }
                    else if(currentState==11)
                    {
                        // 打完三隻怪，前往下關的門打開
                        EventManager.OpenDoor();
                        // 魔法書消失
                        MagicBook.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Close",true);
                    }
                dialogueEnd = true;
            }
            // stone elf's stage
            else if(EventManager.stage==1)
            {
                if(!dialogueEnd)
                    if(currentState==6)
                    {
                        // 切到攻擊模式 (要打三隻怪)
                        SwitchMode.Mode_Content = "Combat";
                        EventManager.SwitchPlayMode("Combat");
                        EventManager.CreateElf(3);
                        MagicBook.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Close",true);
                    }
                    else if(currentState==7)
                    {
                        EventManager.OpenDoor();
                        MagicBook.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Close",true);
                    }
                dialogueEnd = true;
            }

            // fire elf's stage
            else if (EventManager.stage == 2)
            {
                if(!dialogueEnd)
                    if (currentState == 9)
                    {
                        // 切到攻擊模式 (要打三隻怪)
                        SwitchMode.Mode_Content = "Combat";
                        EventManager.SwitchPlayMode("Combat");
                        EventManager.CreateElf(3);
                        MagicBook.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Close", true);
                        //EventManager.StartDialogue(10);
                        
                        Debug.Log("FINISHHHHHHHHHHHH??????");
                        
                    }
                    else if (currentState == 10)
                    {
                        // 打完三隻怪，前往下關的門打開
                        
                        Debug.Log("OPEN THE DOORRRRRRRRRRRRRRR");
                        EventManager.OpenDoor();
                        MagicBook.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Close", true);
                        


                    }
                //if(currentState == 3)
                //{
                //    EventManager.DialogueState = 3;

                //    EventManager.StartDialogue(4);

                //}   
                dialogueEnd = true;
            }
        }
    }
}
