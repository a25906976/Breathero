using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gesture_active_border : MonoBehaviour
{   

    public bool statement_1 = false;
    

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
    public float first_active_Time, holding_time = 0;   
    public bool ready = false;
    

    [Header("Hand")]
    public GameObject Righthand;
    public GameObject Lefthand;
  
    void Start()
    {
      
      for (int i = 0; i < Target.Length; i++)
      {
        Anchor_Center.transform.position = Target[i].transform.position;
        Anchor_Outline.transform.GetChild(0).gameObject.SetActive(false);
        
      }
    }
    
    public void state(bool statement)
    {
        if(statement == true)
        {
          statement_1 = true;
        }
        else
        {
           statement_1 = false;
        }

    }
    
    void Update()
    {
        if (statement_1 == true  && ready == false)
        {
            first_active_Time = Time.time;
            ready = true; 
            //Debug.Log("Ready = true!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            //Debug.Log("Border_first_active_Time: " + first_active_Time);

         }
        if (statement_1 == false)
        {    
            ready = false;
            //Debug.Log("Ready = false");
        }

        if (holding_time >= Needing_Holding_Time_second)
        {
          ready = false;
          
          
        }

        if (statement_1 == true  && ready == true)
        {
          
          Anchor_Outline.transform.GetChild(0).gameObject.SetActive(true);
          Anchor_Center.transform.Rotate(0f, (360f/Needing_Holding_Time_second) * Time.deltaTime, 0f);
          
        
          Vector3 temp = Anchor_Outline.transform.position;
          x = temp.x;
          y = temp.y;
          z = temp.z;

          holding_time = Time.time - first_active_Time;
          //Debug.Log("Border_holding_time: " + holding_time); 
        }
    }


      




    

      
}
