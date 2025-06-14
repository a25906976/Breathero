using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gesture_active_outsphere : MonoBehaviour
{
    public bool statement_2 = false;
    private bool active_scalingY = false;

    [Header("Oursphere")]
    public GameObject outsphere;
    private Material render_outsphere; 
    
    public Color32 color_original = new Color32(107, 121, 154, 255);
    public Color32 colornew = new Color32(103, 149, 255, 255);
    

    public float Start_Y_Scale;
    public float Target_Y_Scale;
    public float timeElapsed;
    public float valueToLerp;
    
    
 

    [Header("Key_Timer")]
    public float Needing_Holding_Time_second = 0f;
    public float first_active_Time, holding_time = 0;   
    public bool ready = false;

    
    // Start is called before the first frame update
    void Start()
    {
        outsphere.SetActive(false);
        outsphere.transform.localScale = new Vector3(1, 0, 1);
        
        render_outsphere = outsphere.GetComponent<MeshRenderer>().materials[0];

    }

    public void statement_step2(bool statement)
    {
        if(statement == true)
        {
          statement_2 = true;
        }
        else
        {
           statement_2 = false;
        }

    }


    
    // Update is called once per frame
    void Update()
    {
        
        if (statement_2 == true  && ready == false)
        {
            first_active_Time = Time.time;
            ready = true; 
            active_scalingY = true;
            //Debug.Log("Ready = true!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            //Debug.Log("Outsphere_first_active_Time: " + first_active_Time);

         }
        if (statement_2 == false)
        {    
            ready = false;
            outsphere.SetActive(false);
            //Debug.Log("Ready = false");
        }

        if (holding_time >= Needing_Holding_Time_second)
        {
          ready = false;
          active_scalingY = false;
          outsphere.SetActive(true);
          outsphere.transform.localScale = new Vector3(1f, 1f, 1f);
          
        }
        

        if (statement_2 == true  && ready == true)
        {
          
          if(timeElapsed < Needing_Holding_Time_second && active_scalingY == true)
          {  
             outsphere.SetActive(true);
             valueToLerp = Mathf.Lerp(Start_Y_Scale, Target_Y_Scale, timeElapsed / Needing_Holding_Time_second);
            
             timeElapsed += Time.deltaTime;
              
             outsphere.transform.localScale = new Vector3(1f, valueToLerp, 1f);
             


          }

          if(timeElapsed == Needing_Holding_Time_second)
          {
            render_outsphere.SetColor("_ColorBottom", colornew);
            render_outsphere.SetFloat("_OutlineWidth", 0.005f);

          }
          else
          {
            
            render_outsphere.SetColor("_ColorBottom", color_original);
            render_outsphere.SetFloat("_OutlineWidth", 0.001f);
            
          }


        }



    }
}
