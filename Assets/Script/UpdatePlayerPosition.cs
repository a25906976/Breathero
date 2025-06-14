using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 當User重新召喚Book時，更新Book的位置
// 綁在 PositionForMagicBook 上
public class UpdatePlayerPosition : MonoBehaviour
{
    [SerializeField] Transform Player;
    void OnEnable()
    {
        transform.position = Player.position;
        transform.rotation = Player.rotation;
    }
    
    // private void Update() {
    //     if((OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp, OVRInput.Controller.RTouch)||
    //                                 OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown, OVRInput.Controller.RTouch)||
    //                                 OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.RTouch)||
    //                                 OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.RTouch)))
    //     {
    //         transform.rotation = Player.rotation;
    //     }
    // }

}

