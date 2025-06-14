using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 綁在Portal上
public class ToNextScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player")
        {
            // 第一關 跳到 第二關
            if (EventManager.stage == 0)
            {
                Debug.Log("LoadScene2");
                SceneManager.LoadScene("Scene2");
            }
                
                 
            // 第二關 跳到 第三關
            else if (EventManager.stage == 1)
            {
                Debug.Log("LoadScene3");
                SceneManager.LoadScene("Scene3");
            }
                

            // 第三關 跳到 第四關
            else if (EventManager.stage == 2)
            {
                Debug.Log("LoadBossScene");
                SceneManager.LoadScene("BossScene");
            }
                

        }
    }
}
