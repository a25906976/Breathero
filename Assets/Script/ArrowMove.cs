using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 綁在 Arrow Object 上
// 顯示/消失 3D Arrow
public class ArrowMove : MonoBehaviour
{
    [SerializeField] GameObject inhaleArrow;
    [SerializeField] GameObject exhaleArrow;
    [SerializeField] GameObject holdArrow;
    [SerializeField] EnergyBarMove fireSlider;
    [SerializeField] EnergyBarMove boxSlider;
    // Update is called once per frame


    void Update()
    {   
        // tree elf's stage
        if(ChangeSkill.currentSkill==0)
        {
            // energyBar為空 則 吸氣
            if(fireSlider.value==0)
            {
                inhaleArrow.SetActive(true);
                exhaleArrow.SetActive(false);
            }
            // energyBar填滿 則 吐氣
            else if(fireSlider.value==2)
            {    
                inhaleArrow.SetActive(false);
                exhaleArrow.SetActive(true);  
            }
        }
        // stone elf's stage
        else if(ChangeSkill.currentSkill==1)
        {
            // energyBar為空 則 吸氣
            if(boxSlider.value==0f)
            {
                inhaleArrow.SetActive(true);
                exhaleArrow.SetActive(false);
            }
            else if(boxSlider.value==4f)
            {
                inhaleArrow.SetActive(false);
                holdArrow.SetActive(true);
            }
            // energyBar填滿 則 吐氣
            else if(boxSlider.value==8f)
            {    
                holdArrow.SetActive(false);
                exhaleArrow.SetActive(true);  
            }
            else if(boxSlider.value>=12f)
            {
                exhaleArrow.SetActive(false);  
            }
        }
    }
}
