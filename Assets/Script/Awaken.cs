using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

// 綁在 Global Volume Object上
// player 甦醒的睜眼效果 (用post processing做的)
public class Awaken : MonoBehaviour
{
    // 睜眼效果是否播完
    public bool Awaked;
    [SerializeField] Volume PostVolume;
    private Vignette closeEyes;
    private Vector2 closeEyesCenter;
    private Vector2 openEyesCenter;

    private void Awake() {
        PostVolume.profile.TryGet<Vignette>(out closeEyes);
    }
    
    void OnEnable()
    {
        Awaked = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // scene載入完成才執行該function
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 前置設定
        closeEyes.color.value = Color.black;
        closeEyes.intensity.value = 1;
        closeEyes.smoothness.value = 1;
        closeEyesCenter = new Vector2(0.5f,-0.3f);
        closeEyes.center.value = closeEyesCenter;
        openEyesCenter = new Vector2(0.5f,0.5f);
        // 睜眼
        StartCoroutine(OpenEyes());
    }
    private IEnumerator OpenEyes()
    {
        float elpasedTime = 0f;
        float elpasedDuration = 5f;
        while(elpasedTime<elpasedDuration)
        {   
            closeEyes.intensity.Interp(1, 0.4f, elpasedTime/elpasedDuration);
            closeEyes.smoothness.Interp(1, 0.2f, elpasedTime/elpasedDuration);
            closeEyes.center.Interp(closeEyesCenter, openEyesCenter, elpasedTime/elpasedDuration);
            elpasedTime += Time.deltaTime;
            yield return null;
        }
        // 睜眼完畢
        Debug.Log("Awake??");
        Awaked = true;
    }

}
