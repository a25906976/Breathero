// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class OrbitMotion : MonoBehaviour
// {
//     public RectTransform orbitingObject;
//     public Transform center;
//     public OrbitPath path;
//     public float radius;
//     private float orbitingObjectPosY;

//     [Range(0f, 1f)] public float orbitProgress = 0f;
//     public float orbitPeriod = 1f;
//     public bool orbitActive = true;

//     private void OnEnable() {
//         EventManager.BookMoveEvent += StartOrbit;
//     }

//     private void OnDisable() {
//         EventManager.BookMoveEvent -= StartOrbit;
//     }

//     public void StartOrbit(bool anticlockwise)
//     {
//         if(orbitingObject==null)
//         {
            
//             orbitActive = false;
//             return;
//         }
      
//         if(anticlockwise)
//             orbitProgress = 0;
//         else
//             orbitProgress = 1;
//         SetOrbitPos();
//         StartCoroutine(AnimateOrbit(anticlockwise));
//     }

//     void SetOrbitPos()
//     {
//         Vector2 orbitPos = path.Evaluate(orbitProgress,radius);
//         orbitingObject.localPosition = new Vector3(orbitPos.x, -0.649f, orbitPos.y);
//         orbitingObject.localEulerAngles = new Vector3(20f, orbitProgress*30, 0f);

//     }
//     IEnumerator AnimateOrbit(bool anticlockwise)
//     {
//         if(orbitPeriod<0.1f)
//         {
//             orbitPeriod = 0.1f;
//         }
//         float orbitSpeed = 1f / orbitPeriod;
//         while(orbitActive)
//         {
            
//             if(anticlockwise){
//                 orbitProgress += Time.deltaTime * orbitSpeed;
//                 if(orbitProgress>=1f)
//                     break;
//             }
//             else{
//                 orbitProgress -= Time.deltaTime * orbitSpeed;
//                 if(orbitProgress<=0f)
//                     break;                
//             }
//             SetOrbitPos();
//             yield return null;
//         }
//         if(anticlockwise)
//             EventManager.StartDialogue(4);
//         else
//             EventManager.StartDialogue(11);
//         yield return null;
//     }
// }
