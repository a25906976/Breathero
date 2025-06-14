using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 創建樹精
// 綁在 EventManager Object 上
public class CreateFirstElf : MonoBehaviour
{
    //前三關產精靈專用
    [SerializeField] GameObject FirstElfPrefab;

    //王關產精靈專用
    [SerializeField] GameObject TreeElfPrefab;
    [SerializeField] GameObject StoneElfPrefab;
    [SerializeField] GameObject FireElfPrefab;
    [SerializeField] Transform All_Little_Elf_PrefabParent;
    [SerializeField] GameObject Litte_Elfs_Initial_Position;
    private GameObject littleElfs;

    [SerializeField] Transform FirstPrefabParent;
    [SerializeField] FirstElfData_SO elfData;
    //public AudioManager02 audioManager;
    private void Start() {
        elfData.index = 0;
    }

    private void OnEnable() {
        EventManager.CreateElfEvent += ShowUp;
    }
    private void OnDisable() {
        EventManager.CreateElfEvent -= ShowUp;
    }

    // 將出現的樹精
    void ShowUp(int number)
    {
        if (EventManager.stage==0 || EventManager.stage==1 || EventManager.stage == 2)
        { 
            // Practice Part (站樁怪)
            if(number==1){
                GameObject FirstElf00 = Instantiate(FirstElfPrefab, elfData.position[elfData.index], Quaternion.identity, FirstPrefabParent);
                //audioManager.Rock(); // 岩石出現，雖然可能沒啥用
                elfData.index++;
                FirstElf00.transform.rotation = Quaternion.Euler(0,90,0);
                StartCoroutine(FadeIn(FirstElf00.transform.Find("Box027").gameObject));
            } 
            if(number==3){
                GameObject FirstElf03 = Instantiate(FirstElfPrefab, elfData.position[elfData.index], Quaternion.identity, FirstPrefabParent);
                elfData.index++;
                GameObject FirstElf04 = Instantiate(FirstElfPrefab, elfData.position[elfData.index], Quaternion.identity, FirstPrefabParent);
                elfData.index++;
                GameObject FirstElf05 = Instantiate(FirstElfPrefab, elfData.position[elfData.index], Quaternion.identity, FirstPrefabParent);
                elfData.index++;            
                //audioManager.Rock(); // 岩石出現，雖然可能沒啥用
                FirstElf03.transform.rotation = Quaternion.Euler(0,90,0);
                FirstElf04.transform.rotation = Quaternion.Euler(0,90,0);
                FirstElf05.transform.rotation = Quaternion.Euler(0,90,0);
                StartCoroutine(FadeIn(FirstElf03.transform.Find("Box027").gameObject));
                StartCoroutine(FadeIn(FirstElf04.transform.Find("Box027").gameObject));
                StartCoroutine(FadeIn(FirstElf05.transform.Find("Box027").gameObject));
            }


        }
        //王關產精靈！
        if (EventManager.stage == 3)
        { 
            // Practice Part (站樁怪)
            if(BossElfController.BossMode == "TreeMode")
            {
                littleElfs = TreeElfPrefab;
            }
            if(BossElfController.BossMode == "StoneMode")
            {
                littleElfs = StoneElfPrefab;
            }
            if(BossElfController.BossMode == "FireMode")
            {
                littleElfs = FireElfPrefab;
            }


            if(number==1){
                
                GameObject FirstElf00 = Instantiate(littleElfs, Litte_Elfs_Initial_Position.transform.GetChild(1).position, Quaternion.identity, All_Little_Elf_PrefabParent);
                FirstElf00.transform.rotation = Quaternion.Euler(0,90,0);
                StartCoroutine(FadeIn(FirstElf00.transform.Find("Box027").gameObject));
            }
            if(number==2){
                GameObject FirstElf01 = Instantiate(littleElfs, Litte_Elfs_Initial_Position.transform.GetChild(0).position, Quaternion.identity, All_Little_Elf_PrefabParent);
                GameObject FirstElf02 = Instantiate(littleElfs, Litte_Elfs_Initial_Position.transform.GetChild(2).position, Quaternion.identity, All_Little_Elf_PrefabParent); 
                FirstElf01.transform.rotation = Quaternion.Euler(0,90,0);
                FirstElf02.transform.rotation = Quaternion.Euler(0,90,0);
                FirstElf01.GetComponent<ThirdElfController>().state = ElfStates.WALK;
                FirstElf02.GetComponent<ThirdElfController>().state = ElfStates.WALK;
                StartCoroutine(FadeIn(FirstElf01.transform.Find("Box027").gameObject));
                StartCoroutine(FadeIn(FirstElf02.transform.Find("Box027").gameObject));
            }
            if(number==3){
                GameObject FirstElf03 = Instantiate(littleElfs, Litte_Elfs_Initial_Position.transform.GetChild(0).position, Quaternion.identity, All_Little_Elf_PrefabParent);
                GameObject FirstElf04 = Instantiate(littleElfs, Litte_Elfs_Initial_Position.transform.GetChild(1).position, Quaternion.identity, All_Little_Elf_PrefabParent);
                GameObject FirstElf05 = Instantiate(littleElfs, Litte_Elfs_Initial_Position.transform.GetChild(2).position, Quaternion.identity, All_Little_Elf_PrefabParent);           
                FirstElf03.transform.rotation = Quaternion.Euler(0,90,0);
                FirstElf04.transform.rotation = Quaternion.Euler(0,90,0);
                FirstElf05.transform.rotation = Quaternion.Euler(0,90,0);
                StartCoroutine(FadeIn(FirstElf03.transform.Find("Box027").gameObject));
                StartCoroutine(FadeIn(FirstElf04.transform.Find("Box027").gameObject));
                StartCoroutine(FadeIn(FirstElf05.transform.Find("Box027").gameObject));
            }


        }


    }

    // 出現的淡入效果
    private IEnumerator FadeIn(GameObject ObjFade)
    {
        Renderer objRenderer = ObjFade.transform.GetComponent<Renderer>();
        Material objMaterial = objRenderer.material;
        Color cTargetColor = objRenderer.material.color;
        Color cInitialColor = new Color(cTargetColor.r,cTargetColor.g,cTargetColor.b,0f);
        float elpasedTime = 0f;
        float elpasedDuration = 2f;
        while(elpasedTime<elpasedDuration)
        {
            elpasedTime += Time.deltaTime;
            objRenderer.material.color = Color.Lerp(cInitialColor,cTargetColor,elpasedTime/elpasedDuration);
            yield return null;
        }
        // switch to opaque mode
        objRenderer.material.SetFloat("_Mode", 0);
        objMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        objMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        objMaterial.SetInt("_ZWrite", 1);
        objMaterial.DisableKeyword("_ALPHATEST_ON");
        objMaterial.DisableKeyword("_ALPHABLEND_ON");
        objMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        objMaterial.renderQueue = -1;
        yield return null;
    }
}
