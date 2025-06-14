using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circular : MonoBehaviour
{
    [Header("Anchor")]
    public GameObject Anchor_Center;
    public GameObject Anchor_Outline;
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public bool i = true;

    // Start is called before the first frame update
    void Start()
    {
        while(i)
        {
          Anchor_Center.transform.Rotate(0f, 360f * Time.deltaTime, 0f);
          Vector3 temp = Anchor_Outline.transform.position;
          x = temp.x;
          y = temp.y;
          z = temp.z;
          
          
        }       
    }

    // Update is called once per frame
    void Update()
    {   
        





    }
}
