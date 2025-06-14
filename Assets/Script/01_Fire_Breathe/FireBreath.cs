using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(AudioSource))] 我還是看不懂作用的一行，但應該是需要保留的（by芷葳）

// 綁在 Mouth Object 上
public class FireBreath : MonoBehaviour
{
    [SerializeField] float fireRate = 0.7f;
    [SerializeField] GameObject FireBallPrefab;
    [SerializeField] OVRInput.Button exhaleButton;
    [SerializeField] OVRInput.Button inhaleButton;
    [SerializeField] OVRInput.Button recordButton;
    [SerializeField] Image IconCoolDown;
    public static bool Inhaling_finish = false;
    public static bool Exhaling_finish = false;

    // 要改變能量條的數值
    public EnergyBarMove energySlider;
    public BreathDetector breathDectector;
    public AudioManager audioManager;

    //火球
    public GameObject fireBalls;

    //public BellyPosition bellyPosition; 
    public static bool exhaling, inhaling, cooldown;
    //private List<Component> bellyPositions = new List<Component>();
    private float lastFireTime = 0f;
    private float elapsedTime;
    private int fireCount = 0;
    private bool setting = false;
    private bool fireRotate = false;
    private float rotateTime = 0f;

    //四顆火球目前的角度
    private Vector3 fireAngle;
    private Vector3 initFireAngle;
    //設置音樂
    private bool hasMusic = false;

    private bool printed = false;

    //硬體測量相關變數
    private CsvWriter csvWriter;
    private int recordCount = 1;
    private int exhaleCount = 1;
    private float ongoingTime = 0f;
    private string breathAction;
    private string yesColumn = "";


    private void Start() {
        csvWriter = new CsvWriter();
        csvWriter.dirPath = "/DataRecord/";
        inhaling =true;
        IconCoolDown.fillAmount = 0;
        initFireAngle = fireBalls.transform.eulerAngles;
        
    }
    void Update()
    {
        if (printed)
        {
            Debug.Log("Recording Data...");
        }
        lastFireTime += Time.deltaTime;
        // 施展火呼吸
        if(ChangeSkill.currentSkill==0)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (!printed)
                {
                    csvWriter.Open("FireBreath" + recordCount.ToString());
                    printed = true;
                    recordCount++;
                }
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                printed = false;
                ongoingTime = 0f;
            }
            breathAction = "None";
            yesColumn = "";
            //if (!setting)
            //{
            //    fireBalls.SetActive(true);
            //    setting = true;
            //}
            // 冷卻
            if (cooldown)
            {
                fireBalls.transform.localEulerAngles = Vector3.zero;
                elapsedTime += Time.deltaTime;
                IconCoolDown.fillAmount = Mathf.Lerp(1f,0f,elapsedTime/3f);
                if(elapsedTime>=3f)
                {
                    elapsedTime = 0f;
                    inhaling = true;
                    cooldown = false;
                    
                    //setting = false;
                }
            }
            // 充能 (吸氣)
            else if(inhaling)
            {
                
                breathDectector.StartInhale();
                //Debug.Log("isInhale22: " + breathDectector.isInhale);
                if (breathDectector.isInhale || OVRInput.Get(inhaleButton, OVRInput.Controller.RTouch))
                {
                    //Debug.Log("kkkooooo ");
                    

                    foreach (Transform child in fireBalls.transform)
                    {
                        child.gameObject.SetActive(true);
                        child.GetComponent<Animator>().enabled = true;
                    }
                    energySlider.value += Time.deltaTime;
                    if (!hasMusic)
                    {
                        breathAction = "Inhale";
                        hasMusic = true;
                        audioManager.FireInhalePlay();
                    }
                    else
                    {
                        audioManager.FireInhaleUnpause();
                    }
                    // 吸滿能量
                    if (energySlider.value>=2){
                        //breathDectector.UpdateMaxBelt();
                        foreach (Transform child in fireBalls.transform)
                        {
                            child.GetComponent<Animator>().SetBool("Move", true);
                        }
                    }
                }
                if (fireBalls.transform.GetChild(3).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && fireBalls.transform.GetChild(3).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FireRight"))
                {
                    exhaling = true;
                    inhaling = false;
                    Inhaling_finish = true;
                    audioManager.FireInhaleStop();
                }

                // 吸氣沒一次吸滿，能量歸零
                else if (!OVRInput.Get(inhaleButton, OVRInput.Controller.RTouch) && energySlider.value < 2 && !breathDectector.isInhale)
                {
                    foreach (Transform child in fireBalls.transform)
                    {
                        child.GetComponent<Animator>().enabled = false;
                    }
                    audioManager.FireInhalePause();
                    Inhaling_finish = false;
                }
            }
            // 耗能 (吐氣)
            else if(exhaling)
            {
                

                breathDectector.isInhale = false;
                breathDectector.StartExhale();
                if (breathDectector.isExhale || OVRInput.Get(exhaleButton, OVRInput.Controller.RTouch))
                {
                    hasMusic = false;
                    
                    // 每0.5秒才能吐一次火球
                    if (!fireRotate){
                        Fire();
                        rotateTime = 0f;
                        fireRotate = true;
                        fireAngle = fireBalls.transform.eulerAngles;

                        energySlider.value -= energySlider.maxValue/4;
                        
                        if (fireCount++ == 3)
                        {
                            exhaling = false;
                            cooldown = true;
                            elapsedTime = 0;
                            breathDectector.isExhale = false;
                            fireCount = 0;      
                            Exhaling_finish = true;
                            exhaleCount = 1;
                            //audioManager.CountDownPlay();
                        }
                        breathDectector.canExhale = false;
                    }
                }
            }
            if(OVRInput.GetDown(recordButton, OVRInput.Controller.RTouch))
            {
                Debug.Log("Start YES");
                yesColumn = "Yes";
            }
            if (printed)
            {
                ongoingTime += Time.deltaTime;
                csvWriter.WriteCsv(new string[] { ongoingTime.ToString(), breathDectector.windSpeed.ToString(), breathDectector.beltResis.ToString(), breathAction, yesColumn.ToString()});
            }
        }
        // 切換到其他技能
        else if(ChangeSkill.currentSkill!=0)
        {
            foreach (Transform child in fireBalls.transform)
            {
                child.gameObject.SetActive(false);
            }
            //audioManager.CountDownStop();
            fireBalls.transform.localEulerAngles = Vector3.zero;
            IconCoolDown.fillAmount = 0f;
            energySlider.value = 0;
            // elapsedTime = 0f;
            fireCount = 0;
            exhaling = false;
            inhaling = true;
            Inhaling_finish = false;
            Exhaling_finish = false;
            hasMusic = false;
            audioManager.FireInhaleStop();
            ongoingTime = 0f;
            printed = false;
            //setting = false;
        }

        if (fireRotate)
        {
            if(rotateTime >= 0.2f && rotateTime <= 0.7f)
            {
                fireBalls.transform.eulerAngles = Vector3.Lerp(fireAngle, fireAngle + new Vector3(0, 0, -90f), (rotateTime-0.2f) / 0.5f);
            }
            if(rotateTime > 0.7f)
            {
                fireRotate = false;
            }
            rotateTime += Time.deltaTime;
        }
    }

    private void Fire(){
        lastFireTime = 0f;
        fireBalls.transform.GetChild(fireCount).gameObject.SetActive(false);
        GameObject fireBallObj = Instantiate(FireBallPrefab, fireBalls.transform.GetChild(fireCount).position, Quaternion.identity);
        fireBallObj.transform.forward = transform.forward;
        audioManager.FireBallAttact();
        breathAction = "Exhale" + exhaleCount.ToString();
        exhaleCount++;
    }

}
