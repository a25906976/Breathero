using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeltCalibration : MonoBehaviour
{
    public OVRInput.Button buttom;
    public ArduinoBasic arduino;
    public BreathDetector breathDetector;

    private float correctTime = 5.0f;
    private float lastTime = 0.0f;
    private float beltResis;

    private List<float> beltPos = new List<float>();
    private bool isPress = false, detecFinish = false;
    // Start is called before the first frame update
    void Start()
    {
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (float.TryParse(arduino.readMessage.Split(' ')[1], out beltResis)) { }
        }
        catch(System.Exception e)
        {

        }
        if (OVRInput.Get(buttom, OVRInput.Controller.LTouch) && !detecFinish)
        {
            if (!isPress)
            {
                isPress = true;
                lastTime = Time.time;
            }
            else
            {
                Debug.Log("Detet belly position...");
                beltPos.Add(beltResis);
            }
            if (Time.time - lastTime > correctTime)
            {
                Debug.Log(FindAvgValue(beltPos));
                breathDetector.maxBelt = FindAvgValue(beltPos);

                beltPos.Clear();
                lastTime = Time.time;
                
                detecFinish = true;
            }
        }
        else
        {
            isPress = false;
            detecFinish = false;
            beltPos.Clear();
        }
    }
    public float FindAvgValue(List<float> list)
    {
        float total_val = 0;
        foreach (float value in list)
        {
            total_val += value;
        }
        return total_val / list.Count;
    }
}
