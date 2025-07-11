using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_AnimatorController : MonoBehaviour
{
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("Start"))
        {
            animator.SetBool("Start", false);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetBool("Start", true);
        }        
    }
}
