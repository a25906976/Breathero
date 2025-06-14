using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellyPosition : MonoBehaviour
{
    public GameObject controllerOnBelly;
    public GameObject bellyCube;
    public OVRInput.Button buttom;

    //private Vector3 bellyPosition;
    private float correctTime = 5.0f;
    private float lastTime = 0.0f;
    private List<Component> bellyPositions = new List<Component>();
    private bool isPress = false, detecFinish = false;
    // Start is called before the first frame update
    void Start()
    {
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
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
                bellyPositions.Add(controllerOnBelly.transform);
            }
            if (Time.time - lastTime > correctTime)
            {
                Debug.Log(FindAveragePosition<Component>(bellyPositions));
                bellyCube.transform.position = FindAveragePosition<Component>(bellyPositions);
                bellyPositions.Clear();
                lastTime = Time.time;
                bellyCube.SetActive(true);
                detecFinish = true;
            }
        }
        else
        {
            isPress = false;
            detecFinish = false;
            bellyPositions.Clear();
        }
    }

    public static Vector3 FindAveragePosition<TComponent>(List<TComponent> components) where TComponent : Component
    {
        Vector3 output = Vector3.zero;
        foreach (TComponent component in components)
        {
            if (component == null)
            {
                continue;
            }
            output += component.transform.position;
        }
        return output / components.Count;
    }
}
