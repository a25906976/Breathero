using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 綁在Mouth上
public class FullBreath : MonoBehaviour
{
    public static bool ShieldEffect = false;
    public static bool Tutorial_Start_Fire = false;

    [SerializeField] Transform leftController;
    [SerializeField] Transform rightController;
    [SerializeField] Transform HMD;

    [SerializeField] OVRInput.Button exhaleButton;
    [SerializeField] OVRInput.Button inhaleButton;
    [SerializeField] OVRInput.Button recordButton;
    [SerializeField] EnergyBarMove energySlider;
    [SerializeField] Image IconCoolDown;
    [SerializeField] Transform Shield;
    [SerializeField] GameObject GlowBall;
    public GameObject Shield_container;

    public BreathDetector breathDetector;

    private float elapsedTime;
    private Material ShieldMaterial;
    
    private bool InhaleSetted;
    private float dissolve;
    // 這個是GlowingBall
    private int GlowBallPos;

    public GameObject mouth;

    public static bool inhaling, exhaling, CoolDown;

    // None、Move、Done
    private string butterfly;
    private float ShieldPositionY;

    //硬體測量相關變數
    private CsvWriter csvWriter;
    private int recordCount = 1;
    private float ongoingTime = 0f;
    private string breathAction;
    private bool printed = false;
    private bool hasBreath = false;
    private string yesColumn = "";

    private void Start()
    {
        csvWriter = new CsvWriter();
        csvWriter.dirPath = "/DataRecord/";
        ShieldMaterial = Shield.GetComponent<Renderer>().material;
        ShieldPositionY = Shield.transform.localPosition.y;
        ShieldMaterial.SetFloat("_Dissolve", -0.3f);
        IconCoolDown.fillAmount = 0;
        GlowBallPos = 4;
        // //設定音樂
        // audiosource = GameObject.FindGameObjectWithTag("Fight Music").GetComponent<AudioSource>();


    }
    private void Update()
    {
        //ChangeSkill.currentSkill = 2;
        if (printed)
        {
            Debug.Log("Recording Data...");
        }
        if (ChangeSkill.currentSkill == 2)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (!printed)
                {
                    csvWriter.Open("FullBreath" + recordCount.ToString());
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

            //mouth.GetComponent<FireBreath>().enabled = false;
            // 集氣的前置設定
            if (!InhaleSetted)
            {        
                Shield.gameObject.SetActive(true);
                ShieldMaterial.SetFloat("_Dissolve", -0.3f);
                ShieldMaterial.SetFloat("_FresnelPower", 1f);
                ShieldMaterial.SetColor("_BaseColor",Color.yellow);
                Shield.localPosition = new Vector3(Shield.localPosition.x, ShieldPositionY, Shield.localPosition.z);
                elapsedTime = 0;
                InhaleSetted = true;
                inhaling = true;
                butterfly = "None";
                return;
            }

            // 手臂相關
            if(butterfly=="Move")
            {
                float tmpY_right = rightController.localPosition.y-HMD.localPosition.y;
                float tmpY_left = leftController.localPosition.y-HMD.localPosition.y;
                Shield.localPosition = new Vector3(Shield.localPosition.x, ShieldPositionY + tmpY_right/1.5f, Shield.localPosition.z);
            }

            // 吸氣
            if (inhaling)
            {
                breathDetector.StartInhale();
                // 長出盾
                if (breathDetector.isInhale || OVRInput.Get(inhaleButton, OVRInput.Controller.RTouch))
                {
                    if (!hasBreath)
                    {
                        breathAction = "Inhale";
                        hasBreath = true;
                    }
                    energySlider.value += Time.deltaTime;
                    if (elapsedTime <= 3f)
                    {
                        elapsedTime += Time.deltaTime;
                        ShieldMaterial.SetFloat("_Dissolve", Mathf.Lerp(-0.3f, 2.7f, elapsedTime/3f));
                        ShieldMaterial.SetFloat("_FresnelPower", Mathf.Lerp(1f, 0.5f, elapsedTime/3f));
                    }
                    else if (elapsedTime >= 3f)
                    {
                        Shield.GetComponent<Animator>().SetBool("Finish_Shield", true);
                        ShieldMaterial.SetFloat("_FresnelPower", 0.4f);
                        breathDetector.ContinueExhale();
                        elapsedTime = 0;
                        ShieldEffect = true;
                        inhaling = false;
                        exhaling = true;
                        hasBreath = false;
                    }
                }
                if((Mathf.Abs(rightController.localPosition.x-HMD.localPosition.x)>=0.5f || Mathf.Abs(rightController.localPosition.z-HMD.localPosition.z)>=0.5f) 
                    // && rightController.localPosition.y-HMD.localPosition.y>=0f
                    // && (Mathf.Abs(leftController.localPosition.x-HMD.localPosition.x)>=0.5f || Mathf.Abs(leftController.localPosition.z-HMD.localPosition.z)>=0.5f)
                    )
                {
                    butterfly = "Move";
                }
            }
            // 吐氣
            else if (exhaling)
            {
                Shield.GetComponent<Animator>().SetBool("Finish_Shield", false);
                breathDetector.ContinueExhale();
                // 盾變色
                if (breathDetector.isExhale || OVRInput.Get(exhaleButton, OVRInput.Controller.RTouch))
                {
                    if (!hasBreath)
                    {
                        breathAction = "Exhale";
                        hasBreath = true;
                    }
                    if (elapsedTime <= 6f)
                    {
                        
                        elapsedTime += Time.deltaTime;

                        ShieldMaterial.SetColor("_BaseColor", Color.Lerp(Color.yellow, Color.green, elapsedTime / 2f));

                        if((Mathf.Abs(rightController.localPosition.x-HMD.localPosition.x)>=0.5f || Mathf.Abs(rightController.localPosition.z-HMD.localPosition.z)>=0.5f) 
                            // && rightController.localPosition.y-HMD.localPosition.y>=0f
                            // && (Mathf.Abs(leftController.localPosition.x-HMD.localPosition.x)>=0.5f || Mathf.Abs(leftController.localPosition.z-HMD.localPosition.z)>=0.5f)
                            )
                        {
                            butterfly = "Move";
                        }
                    }
                    else if(elapsedTime >= 6f)
                    {
                        butterfly = "Done";
                        CoolDown = true;
                        exhaling = false;
                        IconCoolDown.fillAmount = 1;
                        elapsedTime = 0;
                        var new_Shield = Instantiate(Shield.gameObject, Shield.position, Shield.rotation).AddComponent<ShieldController>();
                        new_Shield.transform.parent = Shield_container.transform;
                        Shield.gameObject.SetActive(false);
                        hasBreath = false;
                    }
                }
            }

            if (OVRInput.GetDown(recordButton, OVRInput.Controller.RTouch))
            {
                Debug.Log("Start YES");
                yesColumn = "Yes";
            }
            if (printed)
            {
                ongoingTime += Time.deltaTime;
                csvWriter.WriteCsv(new string[] { ongoingTime.ToString(), breathDetector.windSpeed.ToString(), breathDetector.beltResis.ToString(), breathAction, yesColumn.ToString() });
            }
        }
        // 還未施展完Full Breath就切到別的技能
        else if (!CoolDown && ChangeSkill.currentSkill != 2)
        {
            ShieldMaterial.SetFloat("_Dissolve", -0.3f);
            energySlider.value = 0f;
            InhaleSetted = false;
            inhaling = false;
            exhaling = false;
            printed = false;
            ongoingTime = 0f;
            hasBreath = false;
        }

        // cool down
        if (CoolDown)
        {
            elapsedTime += Time.deltaTime;
            IconCoolDown.fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / 3f);
            if (elapsedTime >= 3f)
            {
                CoolDown = false;
                InhaleSetted = false;
            }
        }
    }


}
