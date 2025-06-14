using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour
{
    float elapsedTime = 0f;
    Material ShieldMaterial;
    TextMeshProUGUI CountDownText;

    private void OnEnable() {
        ShieldMaterial = GetComponent<Renderer>().material;
        CountDownText = transform.Find("CountDown/Text").GetComponent<TextMeshProUGUI>();
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    private void Update() {
        elapsedTime += Time.deltaTime;
        if(elapsedTime<=24f)
        {
            CountDownText.text = Mathf.Ceil(24-elapsedTime)+"s";
        }
        else if(elapsedTime<=26f)
        {
            CountDownText.text ="";
            ShieldMaterial.SetFloat("_Dissolve", Mathf.Lerp(2.7f, 0f, (elapsedTime-24f) / 2f));
        }
        else if(elapsedTime>=26f)
        {
            Debug.Log("elapsedTime"+elapsedTime);
            Destroy(gameObject);
        }
    }

}
