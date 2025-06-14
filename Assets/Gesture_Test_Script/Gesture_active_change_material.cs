using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gesture_active_change_material : MonoBehaviour
{
    public bool statement = false;

    [Header("Hand")]
    public GameObject hand;
    
    private Material render_hand; 
    [Header("Outline_Color")]
    public Color32 color_original = new Color32(137, 137, 137, 255);
    public Color32 colornew = new Color32(255, 213, 0, 255);
    
    

    // Start is called before the first frame update
    void Start()
    {
      render_hand = hand.GetComponent<SkinnedMeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {



    }
    
    public void Material_changing(bool statement)
    {
        if (statement == true)
        {
          render_hand.SetColor("_OutlineColor", colornew);
    
          render_hand.SetFloat("_OutlineWidth", 0.001f);
 
         
        }
        else
        {
          render_hand.SetColor("_OutlineColor", color_original);

          render_hand.SetFloat("_OutlineWidth", 0.00093f);
   
        }

        

    }


}
