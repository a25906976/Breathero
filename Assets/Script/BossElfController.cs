using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossElfController : MonoBehaviour
{
    
    [SerializeField] public List<Material> BossMats;
    [SerializeField] GameObject dodgeBox;
    // None、Start、FireMode、TreeMode、StoneMode、End
    public static string BossMode;
    private SkinnedMeshRenderer meshRenderer;
    private FirstElfController treeController;
    private SecondElfController stoneController;
    private ThirdElfController fireController;
    private CapsuleCollider ElfCollider;
    private Animator anim;
    private bool chaging;
    
    public AudioManager audioManager;

    private bool IsCreateLittleElfs = false;
    

    // 被擊倒的次數
    private int Count;

    void Start()
    {
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        treeController = GetComponent<FirstElfController>();
        treeController.isBoss = true;
        stoneController = GetComponent<SecondElfController>();
        stoneController.isBoss = true;
        fireController = GetComponent<ThirdElfController>();
        fireController.isBoss = true;
        ElfCollider = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        BossMode = "None";
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        
        // 魔王死亡
        if(BossMode == "End")
        {
            if(animInfo.IsName("StandUp"))
            {
                if(animInfo.normalizedTime>=1f)
                {
                    ElfCollider.isTrigger = true;
                    anim.SetBool("Dead",true);
                    StartCoroutine(FadeOut(transform.Find("Box027").gameObject));
                    Destroy(gameObject, 3f);
                    BossMode = "None";
                }
            }

        }
        else if(BossMode=="Start")
        {
            BossMode = "FireMode"; 
        }
        // 切到火模式
        else if(BossMode == "FireMode")
        {
            if(fireController.BossChangeMode)
            {
                // 六次都打完了
                if(Count==5)
                {
                    BossMode = "End";
                    anim.SetBool("StandUp",true);
                    Count++;
                }
                else if(!chaging && !animInfo.IsName("DodgeBack"))
                {
                    chaging = true;
                    fireController.enabled = false;
                    anim.SetBool("DodgeBack",true);
                }
                if(animInfo.IsName("DodgeBack") && animInfo.normalizedTime>=1f)
                {
                    if(!animInfo.IsName("ChangeType"))
                        anim.SetBool("ChangeType",true);
                }
                if(animInfo.IsName("ChangeType"))
                {
                    
                    if(animInfo.normalizedTime>=0.1f)
                    { 
                        audioManager.BossChange();
                    }
                    if(animInfo.normalizedTime>=0.5f)
                    {
                        
                        Material[] TmpMats = meshRenderer.materials;
                        TmpMats[2] = BossMats[1];
                        TmpMats[3] = BossMats[2];
                        meshRenderer.materials = TmpMats;
                        if(animInfo.normalizedTime>=1f)
                        {
                            BossMode = "TreeMode";
                            chaging = false;
                            fireController.enabled = false;
                            fireController.BossChangeMode = false;
                            Count++;
                        }
                    }
                }
            }
            else
            {
                if(!fireController.enabled){
                    fireController.enabled = true;
                }
            }
        }
        else if(BossMode == "TreeMode")
        {
            if(treeController.BossChangeMode)
            {
                if(!chaging && !animInfo.IsName("DodgeBack"))
                {
                    chaging = true;
                    anim.SetBool("DodgeBack",true);
                }
                    
                if(animInfo.IsName("DodgeBack") && animInfo.normalizedTime>=1f)
                {
                    if(!animInfo.IsName("ChangeType"))
                        anim.SetBool("ChangeType",true);
                }
                if(animInfo.IsName("ChangeType"))
                {
                    if(animInfo.normalizedTime>=0.1f)
                    { 
                        audioManager.BossChange();
                    }
                    if(animInfo.normalizedTime>=0.5f)
                    {
                        
                        Material[] TmpMats = meshRenderer.materials;
                        TmpMats[3] = BossMats[3];
                        TmpMats[1] = BossMats[4];
                        meshRenderer.materials = TmpMats;
                        if(animInfo.normalizedTime>=1f)
                        {
                            BossMode = "StoneMode";
                            chaging = false;
                            treeController.enabled = false;
                            treeController.BossChangeMode = false;
                            if (Count == 3)
                            {
                                EventManager.CreateElf(3);
                            }
                            Count++;
                        }

                    }
                }
            }
            else
            {
                if(!treeController.enabled){
                    treeController.enabled = true;
                }
            }
        }
        else if(BossMode == "StoneMode")
        {
            if(stoneController.BossChangeMode)
            {
                if(!chaging && !animInfo.IsName("DodgeBack"))
                {
                    chaging = true;
                    anim.SetBool("DodgeBack",true);
                }
                    
                if(animInfo.IsName("DodgeBack") && animInfo.normalizedTime>=1f)
                {
                    if(!animInfo.IsName("ChangeType"))
                        anim.SetBool("ChangeType",true);
                }
                if(animInfo.IsName("ChangeType"))
                {
                    if(animInfo.normalizedTime>=0.1f)
                    { 
                        audioManager.BossChange();
                    }
                    if(animInfo.normalizedTime>=0.5f)
                    {
                        
                        Material[] TmpMats = meshRenderer.materials;
                        TmpMats[1] = BossMats[5];
                        if(Count==2)
                            TmpMats[3] = BossMats[2];
                        else if(Count==4)
                            TmpMats[2] = BossMats[0];
                        
                        meshRenderer.materials = TmpMats;

                        // 為了把精靈顏色變回來
                        transform.Find("Box027").GetComponent<SkinnedMeshRenderer>().material.color = Color.white;

                        if(animInfo.normalizedTime>=1f)
                        {
                            Debug.Log("Count"+Count);
                            if (Count == 2 )
                            {
                                BossMode = "TreeMode";
                                EventManager.CreateElf(3);
                                
                            }
                                
                            else if (Count == 4)
                            {
                                BossMode = "FireMode";
                                EventManager.CreateElf(2);
                            }
                                
                            chaging = false;
                            dodgeBox.SetActive(false);
                            stoneController.enabled = false;
                            stoneController.BossChangeMode = false;
                            Count++;
                        }
                    }
                    
                }
            }
            else
            {
                if(!stoneController.enabled){
                    stoneController.enabled = true;
                    dodgeBox.SetActive(true);
                }
            }
        }
    }

    private IEnumerator FadeOut(GameObject ObjFade)
    {
        Renderer objRenderer = ObjFade.transform.GetComponent<Renderer>();
        // switch to Fade Mode
        objRenderer.material.SetFloat("_Surface", 1f);
        Material objMaterial = objRenderer.material;
        objMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        objMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        objMaterial.SetInt("_ZWrite", 0);
        objMaterial.DisableKeyword("_ALPHATEST_ON");
        objMaterial.EnableKeyword("_ALPHABLEND_ON");
        objMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        objMaterial.renderQueue = 3000;
        Color cInitialColor = objRenderer.material.color;
        Color cTargetColor = new Color(255f,0f,0f,0f);
        float elpasedTime = 0f;
        float elpasedDuration = 3f;
        while(elpasedTime<elpasedDuration)
        {
            elpasedTime += Time.deltaTime;
            objRenderer.material.color = Color.Lerp(cInitialColor,cTargetColor,elpasedTime/elpasedDuration);
            yield return null;
        }
    }
}
