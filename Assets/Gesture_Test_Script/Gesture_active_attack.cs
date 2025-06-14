using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gesture_active_attack : MonoBehaviour
{


    public bool statement_3 = false;
    
    [Header("Attack")]
    public GameObject Sprite;
    
    private Animator sprite_animator;
    private Material sprite_material;
    public float Material_fade_out_Time = 40.0f;
    public float timeElapsed;
    public float valueToLerp;
    
    // Start is called before the first frame update
    void Start()
    {
        sprite_animator = Sprite.GetComponent<Animator>();
    
        sprite_material = Sprite.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[0];
    }


    public void statement_step3(bool statement)
    {
        if(statement == true)
        {
          statement_3 = true;
        }
        else
        {
           statement_3 = false;
        }

        if (statement_3 == true)
        {
          sprite_animator.SetTrigger("FallingDown");

        }
        else
        {
          sprite_animator.SetTrigger("Shaking");
        }

    }

    
    // Update is called once per frame
    void Update()
    {

      if (statement_3 == true)
      {
         timeElapsed += Time.deltaTime;
         if(timeElapsed < Material_fade_out_Time)
        
          { 
            float alpha = sprite_material.color.a;
            float aValue = 0;
         
            valueToLerp = Mathf.Lerp(alpha, aValue, timeElapsed / Material_fade_out_Time);
          

            sprite_material.color = new Color(sprite_material.color.r, sprite_material.color.g, sprite_material.color.b, valueToLerp);
          
          

          }
          else
          {
            Sprite.SetActive(false);
          }


      }

    }



}
