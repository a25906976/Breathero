using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallShield : MonoBehaviour
{
    [SerializeField] GameObject vfxHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FireBall")
        {
            Debug.Log("FIIRE");
            // 播火球消散特效
            GameObject tmp = Instantiate(vfxHit, transform.position, Quaternion.identity);
            Destroy(tmp, 0.4f);
            Destroy(other.gameObject);
        }
    }
}
