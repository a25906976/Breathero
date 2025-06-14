using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 綁在 FullBreathGlowPoints 上
public class UpdataGlowPointsPosition : MonoBehaviour
{
    [SerializeField] Transform Player;
    private void OnEnable() {
        transform.position = Player.position;
        transform.rotation = Player.rotation;
    }
    private void Update() {
        // 顯示在Player的視野前方
        transform.position = Player.position;

        // if((OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp, OVRInput.Controller.RTouch)||
        //                             OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown, OVRInput.Controller.RTouch)||
        //                             OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.RTouch)||
        //                             OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.RTouch)))
        // {
        //     transform.rotation = Player.rotation;
        // }
    }
}
