// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.VFX;

// // 綁在GlowingBall的Sphere上
// public class GlowingBallMove : MonoBehaviour
// {
//     // 碰到時的發光特效
//     [SerializeField] VisualEffect vfx;
//     [SerializeField] FullBreath breath;
//     private Animator anim;
//     private AnimatorStateInfo animInfo;
//     private float elapsedTime;
//     private bool moving;
//     public int GlowBallPosition;
    
    
//     void Start()
//     {
//         anim = transform.parent.GetComponent<Animator>();
//         GlowBallPosition = 1;
//         // 0 是 layer index，即base layer
//         animInfo = anim.GetCurrentAnimatorStateInfo(0);
//         moving = false;
//     }

//     // private void Update() {
//     //     elapsedTime += Time.deltaTime;
//     //     if(elapsedTime>animInfo.length)
//     //     {
//     //         moving = false;
//     //     }
//     // }

//     // private void OnTriggerStay(Collider other) {
//     //     // 要動畫播完(光球移動完)，光球才會繼續往下個位置移動
//     //     if(other.tag == "GameController" && !moving)
//     //     {
//     //         moving = true;
//     //         elapsedTime=0;
//     //         TouchTheBall();
//     //         breath.GenerateShield();
//     //     }
//     // }


//     private void OnTriggerEnter(Collider other) {
//         // 要動畫播完(光球移動完)，光球才會繼續往下個位置移動
//         if(other.tag == "GameController")
//         {
//             TouchTheBall();
//             breath.GenerateShield();
//         }
//     }

//     public void TouchTheBall()
//     {
//         if(GlowBallPosition==1)
//         {
//             anim.SetBool("Point04to01",false);
//             anim.SetBool("Point01to02",true);
//             vfx.Play();
//         }
//         else if(GlowBallPosition==2)
//         {
//             anim.SetBool("Point01to02",false);
//             anim.SetBool("Point02to03",true);
//             vfx.Play();
//         }
//         else if(GlowBallPosition==3)
//         {
//             anim.SetBool("Point02to03",false);
//             anim.SetBool("Point03to04",true);
//             vfx.Play();
//         }
//         else if(GlowBallPosition==4)
//         {
//             anim.SetBool("Point03to04",false);
//             anim.SetBool("Point04to01",true);
//             vfx.Play();
//         }
//         GlowBallPosition = GlowBallPosition==4 ? 1 : GlowBallPosition+1 ;
//     }
// }
