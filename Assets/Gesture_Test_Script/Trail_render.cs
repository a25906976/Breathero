using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail_render : MonoBehaviour
{
    [Header("Target")]    
    [SerializeField] public GameObject[] Target;

    [Header("Anchor")]
    public GameObject Anchor_Center;
    public GameObject Anchor_Outline;
    public GameObject Sphere;
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    
    [Header("Key_Timer")]
    public float Needing_Holding_Time_second = 0f;
    public float  first_press_Time, holding_time = 0;   
    public bool ready = false;
    public bool stop_sign = true;

    [Header("Hand")]
    public GameObject Righthand;
    public GameObject Lefthand;
    


    // Start is called before the first frame update
    void Start()
    {
      for (int i = 0; i < Target.Length; i++)
      {
        Anchor_Center.transform.position = Target[i].transform.position;
        Anchor_Outline.transform.GetChild(0).gameObject.SetActive(false);
      }


      
    }

    // Update is called once per frame
    void Update()
    {   

      
      if (Input.GetKeyDown(KeyCode.Space) && ready == false)
        {
          first_press_Time = Time.time;
          ready = true; 
          
          Debug.Log("first_press_Time: " + first_press_Time);
        
        }

      if (Input.GetKeyUp (KeyCode.Space))
        {
          ready = false;
        }

      if (holding_time >= Needing_Holding_Time_second)
        {
          ready = false;
          Sphere.SetActive(true);
        }

      //Press and hold Space
      if (Input.GetKey(KeyCode.Space) && ready == true )
      {
        Anchor_Outline.transform.GetChild(0).gameObject.SetActive(true);
        Anchor_Center.transform.Rotate(0f, (360f/Needing_Holding_Time_second) * Time.deltaTime, 0f);
        Vector3 temp = Anchor_Outline.transform.position;
        x = temp.x;
        y = temp.y;
        z = temp.z;

        holding_time = Time.time - first_press_Time;
        Debug.Log("holding_time: " + holding_time); 
      }



    }
}
