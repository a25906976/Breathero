using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magicroll_animator : MonoBehaviour
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
        if (animator.GetBool("start"))
        {
            animator.SetBool("start", false);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetBool("start", true);
        }        
    }
}
