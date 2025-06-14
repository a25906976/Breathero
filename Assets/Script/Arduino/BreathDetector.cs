using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BreathDetector : MonoBehaviour
{
    //public TextMesh Breathe_statement;
    //public GameObject left_controller;
    public ArduinoBasic arduino;
    //public float detectThreshold = 0.003f;
    public float exhaleThreshold = 0.3f;
    public float rateThreshold = 0.3f;
    public float holdThreshold = 0f;
    public float maxBelt;
    public float avgBelt;
    public float beltOffset;
    public float alphaBeltOffset;
    public float betaBeltOffset;
    public float winSpeedGap = 0.3f;
    public Vector3 hand;

    private bool bellyTrigger;
    private float triggerLeft;
    private float triggerRight;
    private float time = 0.1f;
    private float lastBellyPos;
    public float windSpeed;
    public float beltResis = 0f;
    public float windRate = 0f;
    public bool isInhale = false;
    public bool isExhale = false;
    public bool canExhale = true;
    public bool isHold = false;
    private List<float> windSpeedList = new List<float>();
    private List<float> avgBeltList = new List<float>();




    private void Awake()
    {
        Invoke("getBellyPos", time);
    }

    private void OnDestroy()
    {
        arduino.OnApplicationQuit();
    }
    // Start is called before the first frame update
    void Start()
    {
        //lastBellyPos = left_controller.transform.localRotation.eulerAngles.x;
        try
        {
            if (float.TryParse(arduino.readMessage.Split(' ')[1], out beltResis))
                avgBelt = beltResis;
        }
        catch (System.Exception e)
        {

        }
        StartCoroutine(getBellyPos());

    }

    // Update is called once per frame
    void Update()
    {
        //hand = (Vector3) left_controller.transform.localRotation.eulerAngles;
        //Debug.Log("isInhale: " + isInhale);
        try
        {
            if (float.TryParse(arduino.readMessage.Split(' ')[0], out windSpeed))
            {
                if (windSpeedList.Count >= 10)
                {
                    windSpeedList.RemoveAt(0);
                    windSpeedList.Add(windSpeed);
                    UpdateWindRate();
                }
                else
                {
                    windSpeedList.Add(windSpeed);
                }
            }
            if (float.TryParse(arduino.readMessage.Split(' ')[1], out beltResis)) { }
        }

        catch (System.Exception e)
        {

        }
        if (avgBeltList.Count > 500)
        {
            avgBelt = avgBeltList.Average();
            avgBeltList.Clear();
        }
        if (beltResis != 0f && !isInhale) avgBeltList.Add(beltResis);

        triggerLeft = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
        triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);
        if (maxBelt < avgBelt || maxBelt > avgBelt + 1000)
        {
            maxBelt = avgBelt + beltOffset / 2;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (triggerLeft > 0.9f && other.gameObject.name == "trigger_region")
        {
            bellyTrigger = true;
            //Breathe_statement.text = $"yes";
            //Debug.Log("GOGO");
        }
        else
        {
            bellyTrigger = false;
        }
    }

    IEnumerator getBellyPos()
    {
        while (true)
        {
            //lastBellyPos = left_controller.transform.localRotation.eulerAngles.x;
            yield return new WaitForSeconds(time);
        }
    }

    public void StartInhale()
    {
        isExhale = false;
        isHold = false;
        if (beltResis > ((maxBelt + avgBelt) / 2)) isInhale = true;
        else if (beltResis < (avgBelt - beltOffset / 2)) isInhale = false;
    }

    /*public bool ContinueInhale()
    {
        StartInhale();
        if(beltResis < avgBelt)
        {
            isInhale = false;
            return false;
        }
        if (windSpeed > inhaleThreshold && isInhale)
            return true;
        else return false;
    }*/

    public void StartExhale()
    {
        isInhale = false;
        isHold = false;
        if (canExhale)
        {
            if (windRate > rateThreshold)
            {
                isExhale = true;
                canExhale = false;
            }
            else if (windRate < rateThreshold || beltResis > (avgBelt)) isExhale = false;

        }
        else { isExhale = false; }
        if (/*beltResis > (avgBelt + beltOffset / 5) ||*/ windRate < -0.05)
        {
            canExhale = true;
        }
    }


    public void ContinueExhale()
    {
        isInhale = false;
        isHold = false;
        if (windSpeed > exhaleThreshold ) isExhale = true;
        if (windSpeed < exhaleThreshold ) isExhale = false;
    }

    public void StartHold()
    {
        isInhale = false;
        isExhale = false;
        if (windRate < holdThreshold) isHold = true;
        else if (windRate > rateThreshold) isHold = false;
    }

    public void UpdateMaxBelt()
    {
        if (float.TryParse(arduino.readMessage.Split(' ')[1], out beltResis))
        {
            maxBelt = beltResis;
        }
    }

    public void UpdateWindRate()
    {
        windRate = (windSpeedList[windSpeedList.Count - 1] - windSpeedList[0]) / windSpeedList.Count;
    }
    /*public bool inhale()
    {
        if (float.TryParse(arduino.readMessage.Split(' ')[0], out windSpeed))
        {
            //Debug.Log(windSpeed);
        }
        if (float.TryParse(arduino.readMessage.Split(' ')[1], out beltResis))

        //Debug.Log($"IN, belly: {bellyTrigger}, left_controller: {left_controller.transform.localPosition.z}, lastBelly: {lastBellyPos.z}");

        if (bellyTrigger && windSpeed > inhaleThreshold)
        {
            if((lastBellyPos - left_controller.transform.localRotation.eulerAngles.x) > detectThreshold)
            {
                if (isInhale == false)
                {
                    lastInhaleTime = Time.time;
                    isInhale = true;
                }
                Debug.Log($"IN1, belly: {bellyTrigger}, left_controller: {left_controller.transform.localRotation.eulerAngles.x}, lastBelly: {lastBellyPos}");
                lastWindSpeed = windSpeed;
                return true;
            }
            else if(Time.time - lastInhaleTime >= minInhaleTime)
            {
                if(left_controller.transform.localRotation.eulerAngles.x - lastBellyPos > detectThreshold+0.2f)
                Debug.Log($"IN2, belly: {bellyTrigger}, left_controller: {left_controller.transform.localRotation.eulerAngles.x}, lastBelly: {lastBellyPos}");
                lastWindSpeed = windSpeed;
                return true;
            }
        }
        isInhale = false;
        if (lastWindSpeed - windSpeed < 0.2)
        {
            lastInhaleTime = Time.time;
        }
        lastWindSpeed = windSpeed;


        //isInhale = false;
        return false;
    }

    public bool exhale()
    {
        if (float.TryParse(arduino.readMessage.Split(' ')[0], out windSpeed))
        {
            //Debug.Log(windSpeed);
        }
        if (float.TryParse(arduino.readMessage.Split(' ')[1], out beltResis))

        if (bellyTrigger && windSpeed > exhaleThreshold && left_controller.transform.localRotation.eulerAngles.x > lastBellyPos)
        {
            Debug.Log($"OUT1, belly: {bellyTrigger}, left_controller: {left_controller.transform.localRotation.eulerAngles.x}, lastBelly: {lastBellyPos}");
            //Debug.Log(windSpeed);
            if (left_controller.transform.localRotation.eulerAngles.x - lastBellyPos> detectThreshold)
            {
                if (Time.time - lastTime > exhaleGapTime && sw1 != sw2)
                {
                    Debug.Log($"OUT1, belly: {bellyTrigger}, left_controller: {left_controller.transform.localRotation.eulerAngles.x}, lastBelly: {lastBellyPos}");
                    //isExhale = true;
                    isInhale = false;
                    sw1 = sw2;
                    lastTime = Time.time;
                    lastWindSpeed = windSpeed;
                    return true;
                }
            }
        }

        lastWindSpeed = windSpeed;
        sw1 = !sw2;
        return false;
    }*/
}
