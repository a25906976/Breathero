using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 綁在AttackRange Object上
// 偵測Range內有無攻擊目標
public class AttackRange : MonoBehaviour
{
    public bool targetInRange;
    // 可攻擊範圍
    [SerializeField] float AttackRadius=3.73f;
    private ParticleSystem.MainModule rangeCircle;
    private void Awake() {
        rangeCircle = GetComponentInChildren<ParticleSystem>().main;
    }

    private void Update() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, AttackRadius);
        foreach(var target in colliders)
        {
            if(target.CompareTag("Elf"))
            {   
                rangeCircle.startColor =  Color.red;
                targetInRange = true;
                break;
            }
            else{
                rangeCircle.startColor =  Color.green;
                targetInRange = false;
            }
        }  
    }

    // 僅在editor mode顯示
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);   
    }
}
