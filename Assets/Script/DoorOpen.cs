using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    private float elapsedTime;
    private void OnEnable() {
        EventManager.DoorOpenEvent += Open;
    }

    private void OnDisable() {
        EventManager.DoorOpenEvent -= Open;
    }

    private void Open()
    {
        elapsedTime = 0f;
        StartCoroutine(HingeRotate());
    }

    IEnumerator HingeRotate()
    {
        while(elapsedTime<3f){
            Debug.Log("transform.rotation"+transform.rotation);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 90, 0), Quaternion.Euler(0, 180, 0), elapsedTime/3f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
