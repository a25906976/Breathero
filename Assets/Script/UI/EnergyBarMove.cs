using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 顯示 能量條的數值變化
// 綁在 Fill_Image Object 上
public class EnergyBarMove : MonoBehaviour
{
    public float value=0;
    public float maxValue;
    public float minValue;

    private float stepValue;

    [SerializeField] RectTransform fillArea;

    private void Start() {
        value = 0;
    }

    // just for unity inspector
    private void OnValidate() {
        stepValue = value/maxValue;
        fillArea.offsetMin = new Vector2(0 - 20*stepValue,fillArea.offsetMin.y);
        fillArea.offsetMax = new Vector2(-175 + 196*stepValue,fillArea.offsetMax.y);     
    }

    private void Update() {
        if(value>maxValue)
        {
            value = maxValue;
        }
        else if(value<minValue)
        {
            value = minValue;
        }
        stepValue = value/maxValue;
        // 視覺上的能量條變化
        fillArea.offsetMin = new Vector2(0 - 20*stepValue,fillArea.offsetMin.y);
        fillArea.offsetMax = new Vector2(-175 + 196*stepValue,fillArea.offsetMax.y);   
    }

}
