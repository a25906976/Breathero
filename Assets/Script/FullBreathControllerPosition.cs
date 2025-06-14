using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullBreathControllerPosition : MonoBehaviour
{
    [SerializeField] Transform leftController;
    [SerializeField] Transform rightController;
    [SerializeField] Transform HMD;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Debug.Log("left-HMD");
        Debug.Log(leftController.localPosition - HMD.localPosition);
        Debug.Log("right-HMD");
        Debug.Log(rightController.localPosition - HMD.localPosition);
        Debug.Log("HMD-left");
        Debug.Log(HMD.localPosition-leftController.localPosition);
        Debug.Log("HMD-right");
        Debug.Log(HMD.localPosition-rightController.localPosition);
        
    }
}
