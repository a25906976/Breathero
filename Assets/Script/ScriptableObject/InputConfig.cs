using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="InputConfig", menuName="ScriptableObjects/InputConfig")]
public class InputConfig : ScriptableObject
{
    // enum
    // witch button
    [SerializeField] OVRInput.Button overInputButton;
    // witch contorller
    [SerializeField] OVRInput.Controller ovrController = OVRInput.Controller.Active;
    
    public bool GetDown()
    {
        return OVRInput.GetDown(overInputButton, ovrController);
    }
    public bool GetUp()
    {
        return OVRInput.GetUp(overInputButton, ovrController);
    }
    public bool Get()
    {
        return OVRInput.Get(overInputButton, ovrController);
    }
}
