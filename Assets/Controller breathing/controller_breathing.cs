using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller_breathing : MonoBehaviour
{
    [Header("Tracking Points")]
    public GameObject right_controller_track_point;
    public GameObject left_controller_track_point;
    public GameObject hmd_tracking_point;
    public GameObject belly_point;

    [Header("Trigger Statement")]
    public bool left_controller_triggered = false;
    public bool right_controller_triggered = false;
    public float X_axis;
    public float Y_axis;
    public float Z_axis;
    

    
    [Header("Text")]
    public TextMesh LController_triggered;
    public TextMesh RController_triggered; 
    public TextMesh Movement_Logtext;
    public TextMesh Breathe_statement;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float triggerLeft = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
        float triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);
        

        if(triggerLeft > 0.9f)
          {
            LController_triggered.text = "True";
            left_controller_triggered = true;
          
          }
        else
          {
            LController_triggered.text = "False";
            left_controller_triggered = false;
          }

        if (left_controller_triggered == true)
          { 
            Vector3 Local_position = right_controller_track_point.transform.position;
            X_axis = Local_position.x;
            Y_axis = Local_position.y;
            Z_axis = Local_position.z;
            Movement_Logtext.text = $"X-axis:{Local_position.x}, Y-axis:{Local_position.y}, Z-axis:{Local_position.z}";

          }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "trigger_region")
        {
            Breathe_statement.text = $"yes";
            Debug.Log("GOGO");
        }
    }
}
