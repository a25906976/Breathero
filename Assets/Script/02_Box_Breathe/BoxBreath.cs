using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

// 綁在Mouth上
public class BoxBreath : MonoBehaviour
{
    // 時停生效與否 (不要在該script外使用)
    private bool BoxEffect;
    // 視野前方四腳錨點的parent (預覽用)
    [SerializeField] Transform RectPointsPreview;

    // 生成的結界錨點
    private Transform RectPoints;
    // // 視野前方的四角錨點
    // private List<Transform> points;
    
    // 畫筆
    [SerializeField] GameObject brushPrefab;
    [SerializeField] OVRInput.Button exhaleButton;
    [SerializeField] OVRInput.Button inhaleButton;
    [SerializeField] OVRInput.Button recordButton;
    [SerializeField] OVRInput.Button holdButton;
    // 預覽的結界箱 (隨玩家視野移動)
    [SerializeField] GameObject BoxPreview;
    // 結界線
    [SerializeField] GameObject RectLinePrefab;
    [SerializeField] EnergyBarMove energySlider;
    [SerializeField] Image IconCoolDown;

    [SerializeField] Material White;

    //呼吸硬體
    public BreathDetector breathDectector;

    // 四角錨點的位置
    private List<Vector3> fixedPoints;
    private float elapsedTime;
    private TrailRenderer RectTrail;
    private MeshFilter RectMeshFilter;
    private Material BoxMaterial;
    private bool ChargeSetted, InhaleSetted, HoldSetted, HoldSetted2, ExhaleSetted;
    public static bool inhaling, holding, exhaling, holding2, CoolDown;
    private bool canHold = false;

    // 畫結界線用
    private Transform brush;
    // 生成的結界線
    private Transform RectLine;
    // 生成的結界箱
    private Transform Box;
    // 設置音樂
    public AudioManager audioManager;
    public bool musicPlaying = false;

    //硬體測量相關變數
    private CsvWriter csvWriter;
    private int recordCount = 1;
    private float ongoingTime = 0f;
    private string breathAction;
    private bool printed = false;
    private string yesColumn = "";

    private void Start() {
        csvWriter = new CsvWriter();
        csvWriter.dirPath = "/DataRecord/";
        IconCoolDown.fillAmount = 0;
        BoxPreview.SetActive(false);
        
        // 設定音樂
        // audiosource = GameObject.FindGameObjectWithTag ("Fight Music").GetComponent<AudioSource> ();
    }

    private void OnEnable() 
    {
        // 地板結界方形的四個角
        fixedPoints = new List<Vector3>(); 
        fixedPoints.Add(Vector3.zero);
        fixedPoints.Add(Vector3.zero);
        fixedPoints.Add(Vector3.zero);
        fixedPoints.Add(Vector3.zero);    
    }

    private void Update() {
        //ChangeSkill.currentSkill = 1;
        if (printed)
        {
            Debug.Log("Recording Data...");
        }

        if (ChangeSkill.currentSkill==1)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (!printed)
                {
                    csvWriter.Open("BoxBreath" + recordCount.ToString());
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

            // 集氣的前置設定
            if (!ChargeSetted)
            {
                // BoxPreview一直綁在Player上，用來預覽而已
                // BoxMaterial = BoxPreview.transform.GetChild(1).GetComponent<Renderer>().material;
                // BoxMaterial.SetFloat("_Tall",0.5f);
                // BoxMaterial.SetFloat("_WallPower",300f);

                BoxPreview.SetActive(false);
                RectPointsPreview.gameObject.SetActive(true);
                // fixedPoints.Clear();
                ChargeSetted = true;
                inhaling = true;
                return;
            }

            if(inhaling)
            {
                breathDectector.StartInhale();
                // 吸氣的前置設定
                if (!InhaleSetted && (OVRInput.GetDown(inhaleButton, OVRInput.Controller.RTouch) || breathDectector.isInhale))
                {
                    // 結界四腳固定
                    if(RectPoints==null)
                    {
                        RectPoints = Instantiate(RectPointsPreview,RectPointsPreview.position, RectPointsPreview.rotation);
                        RectPointsPreview.gameObject.SetActive(false);
                        Debug.Log("fixedPoints"+fixedPoints.Count);
                        for(int i=0;i<RectPoints.childCount;i++)
                        {
                            //Debug.Log("aaa");
                            fixedPoints[i] = RectPoints.GetChild(i).position;
                            Debug.Log("fixedPoints"+fixedPoints[i]);
                        }
                    }
                    // 建一個放在世界座標的結界箱
                    if(Box==null){
                        //Debug.Log("YOYOYOYOYOYOYO");
                        Box = Instantiate(BoxPreview,BoxPreview.transform.position,BoxPreview.transform.rotation).transform;
                        Box.gameObject.SetActive(false);
                        BoxMaterial = Box.GetChild(1).GetComponent<Renderer>().material;
                        BoxPreview.SetActive(false);
                    }

                    // 產生筆刷
                    if(brush==null){
                        brush = Instantiate(brushPrefab,fixedPoints[0],Quaternion.Euler(90f,0f,0f)).transform;
                        RectTrail = brush.GetComponent<TrailRenderer>();
                    }
                    // 產生結界線
                    if(RectLine==null)
                    {
                        RectLine = Instantiate(RectLinePrefab, Vector3.zero, Quaternion.identity).transform;
                        RectMeshFilter = RectLine.GetComponent<MeshFilter>();
                    }
                    brush.position = fixedPoints[0];
                    RectTrail.time = 1000;
                    elapsedTime = 0;
                    InhaleSetted = true;
                    return;
                }
                // 畫結界線
                else if(breathDectector.isInhale || OVRInput.Get(inhaleButton, OVRInput.Controller.RTouch))
                {
                    energySlider.value += Time.deltaTime;
                    if (!musicPlaying)
                    {
                        breathAction = "Inhale";
                        musicPlaying = true;
                        audioManager.BoxInhalePlay();
                    }
                    else
                    {
                        audioManager.BoxInhaleUnpause();
                    }

                    if(elapsedTime<1f)
                    {   
                        elapsedTime += Time.deltaTime;
                        brush.position = Vector3.Lerp(fixedPoints[0],fixedPoints[1],elapsedTime/1f);
                    }
                    else if(elapsedTime<2f)
                    {
                        elapsedTime += Time.deltaTime;
                        brush.position = Vector3.Lerp(fixedPoints[1],fixedPoints[2],(elapsedTime-1f)/1f);
                    }
                    else if(elapsedTime<3f)
                    {
                        elapsedTime += Time.deltaTime;
                        brush.position = Vector3.Lerp(fixedPoints[2],fixedPoints[3],(elapsedTime-2f)/1f);
                    }
                    else if(elapsedTime<=4f)
                    {
                        elapsedTime += Time.deltaTime;
                        brush.position = Vector3.Lerp(fixedPoints[3],fixedPoints[0],(elapsedTime-3f)/1f);
                    }
                    if(elapsedTime>=4f)
                    {
                        // 把畫好結界的Mesh取出
                        RectTrail.BakeMesh(RectMeshFilter.mesh, true);
                        //關音效
                        audioManager.BoxInhaleStop();
                        // 畫筆不需要了
                        RectTrail.time = 0;
                        Destroy(brush.gameObject);
                        brush = null;
                        RectTrail = null;
                        elapsedTime = 0f;
                        inhaling = false;
                        holding = true;
                        musicPlaying = false;
                    }
                }
                else if (!(breathDectector.isInhale || OVRInput.Get(inhaleButton, OVRInput.Controller.RTouch)))
                {
                    audioManager.BoxInhalePause();
                }
            }
            else if(holding)
            {
                //elapsedTime = 0f;
                breathDectector.StartHold();
                if(elapsedTime < 1f)
                {
                    elapsedTime += Time.deltaTime;
                }
                else
                {
                    canHold = true;
                }
                // 閉氣的前置設定
                if (canHold && !HoldSetted && (OVRInput.GetDown(holdButton, OVRInput.Controller.RTouch) || breathDectector.isHold))
                {
                    elapsedTime = 0f;
                    HoldSetted = true;
                    return;
                }
                // 閉氣 (長柱子)
                else if(HoldSetted && (breathDectector.isHold || OVRInput.Get(holdButton, OVRInput.Controller.RTouch)))
                {
                    energySlider.value += Time.deltaTime;
                    if (!musicPlaying)
                    {
                        breathAction = "Hold1";
                        musicPlaying = true;
                        audioManager.BoxHoldPlay();
                    }
                    else
                    {
                        audioManager.BoxHoldUnpause();
                    }

                    if(elapsedTime<=4f)
                    {   
                        elapsedTime += Time.deltaTime;
                        RectPoints.localScale = Vector3.Lerp(Vector3.one, new Vector3(1f, 45f, 1f) ,elapsedTime/4f);
                    }
                    else if(elapsedTime>=4f)
                    {
                        audioManager.BoxHoldStop();
                        for(int i=0;i<RectPoints.childCount;i++)
                        {
                            RectPoints.GetChild(i).GetComponent<MeshRenderer>().material = White; 
                        }
                        canHold = false;
                        holding = false;
                        exhaling = true;
                        musicPlaying = false;
                        
                    }
        
                }
                else if (!(HoldSetted && (breathDectector.isHold || OVRInput.Get(holdButton, OVRInput.Controller.RTouch))))
                {
                    audioManager.BoxHoldPause();
                }
            }
            // 吐氣 (產生結界箱)
            else if(exhaling)
            {
                breathDectector.ContinueExhale();
                // 閉氣的前置設定
                if (!ExhaleSetted && (breathDectector.isExhale || OVRInput.GetDown(exhaleButton, OVRInput.Controller.RTouch)))
                {
                    // 可以看到結界箱
                    Box.gameObject.SetActive(true);
                    BoxMaterial.SetFloat("_Tall",0.5f);
                    BoxMaterial.SetFloat("_WallPower",300f);
                    ExhaleSetted = true;
                    elapsedTime = 0;
                    return;
                }
                else if(breathDectector.isExhale || OVRInput.Get(exhaleButton, OVRInput.Controller.RTouch))
                {
                    // 箱子長出來
                    energySlider.value += Time.deltaTime;
                    if (!musicPlaying)
                    {
                        breathAction = "Exhale";
                        musicPlaying = true;
                        audioManager.BoxInhalePlay();
                    }
                    else
                    {
                        audioManager.BoxInhaleUnpause();
                    }
                    if(elapsedTime<=4f)
                    {
                        elapsedTime += Time.deltaTime;
                        BoxMaterial.SetFloat("_Tall",Mathf.Lerp(0.5f,0.8f,elapsedTime/1f));
                        BoxMaterial.SetFloat("_WallPower",Mathf.Lerp(300f,0f,elapsedTime/4f));
                    }
                    else if(elapsedTime>=4f)
                    {
                        audioManager.BoxInhaleStop();
                        elapsedTime = 0f;
                        exhaling = false;
                        holding2 = true;
                        musicPlaying = false;
                    }
                }
                else if (!(breathDectector.isExhale || OVRInput.Get(exhaleButton, OVRInput.Controller.RTouch)))
                {
                    audioManager.BoxInhalePause();
                }
            }
            // 閉氣2 (箱子變色，消失)
            else if(holding2)
            {
                if(elapsedTime < 0.5f && !canHold)
                {
                    elapsedTime += Time.deltaTime;
                }
                else
                {
                    canHold = true;
                }
                breathDectector.StartHold();

                // 吐氣的前置設定
                if (canHold && !HoldSetted2 && (breathDectector.isHold || OVRInput.GetDown(holdButton, OVRInput.Controller.RTouch)))
                {
                    breathAction = "Hold2";
                    elapsedTime = 0;
                    // 開啟 時停效果
                    Box.GetComponentInChildren<DetectElfInBox>().enabled = true;
                    HoldSetted2 = true;
                    //return;
                }
                
                if(HoldSetted2 && (breathDectector.isHold || OVRInput.Get(holdButton, OVRInput.Controller.RTouch)))
                {
                    energySlider.value += Time.deltaTime;
                    
                    // 結界箱子填滿顏色
                    if(elapsedTime<=2f)
                    {
                        elapsedTime += Time.deltaTime;
                        BoxMaterial.SetFloat("_VoronoiPower",Mathf.Lerp(2f,0f,elapsedTime/2f));
                        // BoxMaterial.SetFloat("_WallPower",Mathf.Lerp(0f,-50f,elapsedTime/2f));
                    }
                    // 結界箱子消散
                    else if(elapsedTime<=4f)
                    {
                        if(RectLine!=null)
                        {
                            Destroy(RectLine.gameObject);
                            RectLine = null;
                        }
                        if(RectPoints!=null)
                        {
                            Destroy(RectPoints.gameObject);
                            RectPoints = null;
                        }
                        if (!musicPlaying)
                        {
                            musicPlaying = true;
                            audioManager.BoxEndPlay();
                        }

                        elapsedTime += Time.deltaTime;
                        //audioManager.FinishBox(); // 消散的音效
                        BoxMaterial.SetFloat("_VoronoiPower",Mathf.Lerp(0f,100f,(elapsedTime-2f)/2f));
                        if(elapsedTime>=3f)
                            BoxMaterial.SetFloat("_Tall",Mathf.Lerp(0.8f,1.2f,(elapsedTime-3f)/1f));
                    }
                    else if(elapsedTime>=4f)
                    {
                        BoxEffect = true;
                        holding2 = false;
                        canHold = false;
                        // 只留Detecor (因為時停效果在箱子消失後仍會持續幾秒)
                        Destroy(Box.GetChild(0).gameObject);
                        Destroy(Box.GetChild(1).gameObject);
                        musicPlaying = false;
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
                csvWriter.WriteCsv(new string[] { ongoingTime.ToString(), breathDectector.windSpeed.ToString(), breathDectector.beltResis.ToString(), breathAction, yesColumn.ToString() });
            }
        }
        // 還未完成Box Breath就切到別的技能
        else if(!BoxEffect && ChangeSkill.currentSkill!=1)
        {
            canHold = false;
            if(RectPoints!=null)
            {
                Destroy(RectPoints.gameObject);
                RectPoints = null;
            }
            if(brush!=null)
            {
                Destroy(brush.gameObject);
                brush = null;
            }
                
            if(RectLine!=null)
            {
                Destroy(RectLine.gameObject);
                RectLine = null;
            }
                
            if(Box!=null)
            {
                Destroy(Box.gameObject);
                Box = null;
            }
                
            energySlider.value = 0f;
            elapsedTime = 0f;
            ChargeSetted = false;
            InhaleSetted = false;
            HoldSetted = false;
            ExhaleSetted = false;
            HoldSetted2 = false;
            inhaling = false;
            holding = false;
            exhaling = false;
            holding2 = false;
            audioManager.BoxHoldStop();
            audioManager.BoxInhaleStop();
            musicPlaying = false;
            printed = false;
        }
        
        // 技能生效
        if(BoxEffect){
            energySlider.value -= Time.deltaTime*0.5f;
            elapsedTime = 0;
            IconCoolDown.fillAmount = 1;
            CoolDown = true;
            BoxEffect = false;
            Box = null;
            return;
        }
        // cool down
        else if(CoolDown)
        {
            elapsedTime += Time.deltaTime;
            IconCoolDown.fillAmount = Mathf.Lerp(1f,0f,elapsedTime/3f);
            if(elapsedTime>=3f)
            {
                CoolDown = false;
                ChargeSetted = false;
                InhaleSetted = false;
                HoldSetted = false;
                ExhaleSetted = false;
                HoldSetted2 = false;
                elapsedTime = 0f;
            }
        }
    }
}
