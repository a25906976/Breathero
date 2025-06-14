using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 第零關，進到circle，開始第一關
// 綁在 StartCircle Object 上
public class EnterStartCircle : MonoBehaviour
{
    [SerializeField] OVRRaycaster rayScript;
    public static bool enter_start_circle = false;
    
    private void Start() {
        gameObject.SetActive(false);
        enter_start_circle = false;
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player")
        {
            
            // 可以點擊魔法書
            rayScript.enabled = true;
            enter_start_circle = true;
            
        }
        
    }
}
