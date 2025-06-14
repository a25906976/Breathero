using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 綁在GlowingBall的Sphere上
public class MeshRenderOrder : MonoBehaviour
{
    private MeshRenderer meshRender;
    void OnEnable()
    {
        meshRender = GetComponent<MeshRenderer>();
        meshRender.sortingOrder = 1;
    }
    
}
