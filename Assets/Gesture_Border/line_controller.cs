using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class line_controller : MonoBehaviour
{

    private LineRenderer linerenderer;

    //[Header("Anchors")]
    //public GameObject first;
    //public GameObject second;
    //public GameObject third;
    //public GameObject forth;
    //public GameObject fifth;

    [SerializeField] private Transform[] anchor_transform;
       
    // Start is called before the first frame update
    void Start()
    {
        linerenderer = GetComponent<LineRenderer>();
        linerenderer.positionCount = anchor_transform.Length;


    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i<anchor_transform.Length; i++)
        {
            linerenderer.SetPosition(i, anchor_transform[i].position);
        }      
        
    }
}
