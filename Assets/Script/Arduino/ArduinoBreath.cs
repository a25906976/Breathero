using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoBreath : MonoBehaviour
{

    public ArduinoBasic arduino;
    private float lastTime;
    private bool sw1 = false, sw2 = true;
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        float a;
        if (float.TryParse(arduino.readMessage, out a))
        {
            if (a > 0.8f)
            {
                if (Time.time - lastTime > 0.8f && sw1 != sw2)
                {
                    Debug.Log("exhalting!");
                    lastTime = Time.time;
                    sw1 = sw2;
                }
            }
            else
            {
                sw1 = !sw2;
            }

        }
    }
}
