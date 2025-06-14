using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gesture_border_all_flow : MonoBehaviour
{   
    public GameObject material_changing_func;
    public GameObject border_running_func;
    public GameObject outsphere_running_func;
    public GameObject attack_running_func;
  
    public bool run_material_change_state = false;
    public bool run_border_state = false;
    public bool run_outsphere_state = false;
    public bool run_attack_state = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void run_material_change(bool run_material_change_state)
    {
      if (run_material_change_state == true)
      {
        material_changing_func.GetComponent<Gesture_active_change_material>().Material_changing(true);
      }
      else
      {
        material_changing_func.GetComponent<Gesture_active_change_material>().Material_changing(false);
      }
      

    }


    public void run_border(bool run_border_state)
    {
      if (run_border_state == true)
      {
         border_running_func.GetComponent<Gesture_active_border>().state(true);
         
      }
      else
      {
         border_running_func.GetComponent<Gesture_active_border>().state(false);
      }
 
    }

    public void run_outsphere(bool run_outsphere_state)
    {
      if (run_outsphere_state == true)
      {
         outsphere_running_func.GetComponent<Gesture_active_outsphere>().statement_step2(true);
         
      }
      else
      {
         outsphere_running_func.GetComponent<Gesture_active_outsphere>().statement_step2(false);
         
      
      }
    }

    public void run_attack(bool run_attack_state)
    {
      if (run_attack_state == true)
      {
         attack_running_func.GetComponent<Gesture_active_attack>().statement_step3(true);
       
      }
      else
      {
         attack_running_func.GetComponent<Gesture_active_attack>().statement_step3(false);
         
      }


    }



}
