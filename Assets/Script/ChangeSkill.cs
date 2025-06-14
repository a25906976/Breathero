using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tree elf's stage 只有一個技能，不須切換
// 綁在第二關(及之後的關卡)的EnergyBar上
public class ChangeSkill : MonoBehaviour
{
    // 控制能量條數值
    [SerializeField] RectTransform FireSlider;
    [SerializeField] RectTransform BoxSlider;
    [SerializeField] RectTransform FullSlider;

    // // 火呼吸時要顯示攻擊範圍
    // [SerializeField] GameObject AttackRange;
    // 箱式呼吸時要顯示四角錨點
    [SerializeField] GameObject RectPoints;
    // // 完全呼吸時要顯示光球
    // [SerializeField] GameObject GlowBall;

    [SerializeField] OVRInput.Button switchButton;

    public static bool changeskill = false;
    // 顯示在玩家面前的能量條位置
    private Vector2 centerPosition;
    
    // 右方的能量條位置(看不到)
    private Vector2 rightPosition;
    private Vector2 rightMove;
    public static int currentSkill;
    public int currentSkillforboss;
    private float lastSwitch;
    // Start is called before the first frame update
    void Start()
    {
        centerPosition = FireSlider.anchoredPosition;
        rightPosition = BoxSlider.anchoredPosition;
        // 切換時，每個能量條要往左移多少
        rightMove = centerPosition - rightPosition;
    }

    void Update()
    {
        // 每次切換技能必須間隔0.6秒
        if (Time.time - lastSwitch > 0.6f && (OVRInput.GetDown(switchButton, OVRInput.Controller.LTouch) || changeskill == true))
        {
            lastSwitch = Time.time;
            // stone elf's stage 只有兩個技能
            if(EventManager.stage==1)
            {
                currentSkill = currentSkill==1 ? 0 : 1;
                switch(currentSkill)
                {
                    // switch Box to Fire
                    case 0:
                        StartCoroutine(MoveSlider(FireSlider,"right2center"));
                        StartCoroutine(MoveSlider(BoxSlider,"center2Left"));
                        // AttackRange.SetActive(true);
                        RectPoints.SetActive(false);
                        break;
                    // switch Fire to Box
                    case 1:
                        StartCoroutine(MoveSlider(BoxSlider,"right2center"));
                        StartCoroutine(MoveSlider(FireSlider,"center2Left"));
                        // AttackRange.SetActive(false);
                        RectPoints.SetActive(true);
                        break;
                }
            }
            // 三個技能的情況
            else
            {
                currentSkill = currentSkill==2 ? 0 : currentSkill+1;
                switch(currentSkill)
                {
                    // switch Full to Fire
                    case 0:
                        BoxSlider.anchoredPosition = rightPosition;
                        StartCoroutine(MoveSlider(FireSlider,"right2center"));
                        StartCoroutine(MoveSlider(FullSlider,"center2Left"));
                        currentSkillforboss = 0;
                        // AttackRange.SetActive(true);
                        // GlowBall.SetActive(false);
                        break;
                    // switch Fire to Box
                    case 1:
                        FullSlider.anchoredPosition = rightPosition;
                        StartCoroutine(MoveSlider(BoxSlider,"right2center"));
                        StartCoroutine(MoveSlider(FireSlider,"center2Left"));
                        currentSkillforboss = 1;
                        // AttackRange.SetActive(false);
                        RectPoints.SetActive(true);
                        // GlowBall.SetActive(false);
                        break;
                    // switch Box to Full
                    case 2:
                        FireSlider.anchoredPosition = rightPosition;
                        StartCoroutine(MoveSlider(FullSlider,"right2center"));
                        StartCoroutine(MoveSlider(BoxSlider,"center2Left"));
                        currentSkillforboss = 2;
                        RectPoints.SetActive(false);
                        // AttackRange.SetActive(false);
                        // GlowBall.SetActive(true);
                        break;
                }
            }

        }
    }

    IEnumerator MoveSlider(RectTransform slider, string action)
    {
        float elapsedTime=0f;
        Vector2 currentPos = slider.anchoredPosition;
        while(elapsedTime<=0.5f)
        {
            elapsedTime += Time.deltaTime;
            slider.anchoredPosition = currentPos + rightMove*(elapsedTime/0.5f);
            yield return null;
        }

        // 只有兩個技能的情況才需要
        if(EventManager.stage==1 && action=="center2Left")
            slider.anchoredPosition = rightPosition;
    }
}
