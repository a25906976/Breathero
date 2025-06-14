using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;


// 綁在 Global Volume Object上
// player 掛掉的閉眼效果 (用post processing做的)
public class Dead_Eye_closing_and_opening : MonoBehaviour
{

    // 閉眼效果是否播完
    public bool Died;
    public GameObject Camera;
    [SerializeField] Volume PostVolume;
    private Vignette closeEyes;
    private Vector2 closeEyesCenter;
    private Vector2 openEyesCenter;
    private Vector3 LifeRotation;
    private Vector3 DiedRotation;
    // Start is called before the first frame update

    private void Awake()
    {
        PostVolume.profile.TryGet<Vignette>(out closeEyes);
    }

    void OnEnable()
    {
        Died = false;
        EventManager.DieEvent += Eyes_Open_or_Close;
    }
    void OnDisable()
    {
        EventManager.DieEvent -= Eyes_Open_or_Close;
    }

    void Eyes_Open_or_Close(bool state)
    {
        if (state == false)
        {
            StartCoroutine(Close_Eyes());
        }
        if (state == true)
        {
            StartCoroutine(Open_Eyes());
        }
    }

    private IEnumerator Open_Eyes()
    {
        float elpasedTime = 0f;
        float elpasedDuration = 5f;
        closeEyesCenter = new Vector2(0.5f, -0.3f);
        openEyesCenter = new Vector2(0.5f, 0.5f);
        closeEyes.center.value = closeEyesCenter;
        while (elpasedTime < elpasedDuration)
        {
            closeEyes.intensity.Interp(1f, 0.44f,  elpasedTime / elpasedDuration);
            closeEyes.smoothness.Interp(1f, 0.2f,  elpasedTime / elpasedDuration);
            closeEyes.center.Interp(closeEyesCenter, openEyesCenter, elpasedTime / elpasedDuration);
            elpasedTime += Time.deltaTime;
            yield return null;
        }
        // 睜眼完畢
        Died = false;
        
    }

    private IEnumerator Close_Eyes()
    {
        float elpasedTime = 0f;
        float elpasedDuration = 5f;
        closeEyesCenter = new Vector2(0.5f, -0.3f);
        openEyesCenter = new Vector2(0.5f, 0.5f);
        closeEyes.center.value = openEyesCenter;
        while (elpasedTime < elpasedDuration)
        {
            closeEyes.intensity.Interp(0.44f, 1f, elpasedTime / elpasedDuration);
            closeEyes.smoothness.Interp(0.2f, 1f, elpasedTime / elpasedDuration);
            closeEyes.center.Interp(openEyesCenter, closeEyesCenter, elpasedTime / elpasedDuration);
            elpasedTime += Time.deltaTime;
            yield return null;
        }
        // 閉眼完畢
        Died = true;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
