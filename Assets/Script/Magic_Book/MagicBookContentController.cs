using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 綁在RollTwoDirections上，卷軸出現/消失時，控制文字和按鈕的顯示
public class MagicBookContentController : MonoBehaviour
{
    [SerializeField] List<GameObject> Contents;
    [SerializeField] GameObject MagicBook;
    private Animator anim;
    private bool open;
    
    void OnEnable()
    {
        anim = GetComponent<Animator>();
        foreach(var content in Contents)
        {
            content.SetActive(false);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        
        if(!open && animInfo.IsName("Open") && animInfo.normalizedTime>=0.5f)
        {
            foreach(GameObject content in Contents)
            {
                content.SetActive(true);
            }
            open = true;
        }
        else if(animInfo.IsName("Close") && animInfo.normalizedTime>=0.3f)
        {
            foreach(GameObject content in Contents)
            {
                content.SetActive(false);
            }
            open = false;
        }
        else if(animInfo.IsName("Disappear")&& animInfo.normalizedTime>=1f)
        {
            anim.SetBool("Close",false);
            MagicBook.SetActive(false);
        }
    }
}
