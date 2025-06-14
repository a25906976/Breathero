using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class TestFire : MonoBehaviour
{
    public List<GameObject> fires;
    public GameObject fireBalls;
    [SerializeField] GameObject FireBallPrefab;
    public float speed = 1f;

    private float time = 0f;
    private Vector3 rotatePoint;
    private bool p = false;
    private Vector3 ro;

    private int fireCount = 0;
    private bool setting = false;
    private bool fireRotate = false;
    private float rotateTime = 0f;
    private Vector3 fireAngle;
    private float lastTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rotatePoint = this.transform.position;
        Debug.Log(rotatePoint);
    }

    // Update is called once per frame
    void Update()
    {
        lastTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.V))
        {
            foreach (GameObject f in fires)
            {
                f.GetComponent<Animator>().SetBool("Move", true);
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (lastTime > 0.7f)
            {
                Fire();
                rotateTime = 0f;
                fireRotate = true;
                fireAngle = fireBalls.transform.eulerAngles;
                fireCount++;
                lastTime = 0f;
            }
        }
        if (fireRotate)
        {
            if (rotateTime > 0.2f && rotateTime < 0.7f)
            {
                fireBalls.transform.eulerAngles = Vector3.Lerp(fireAngle, fireAngle + new Vector3(0, 0, -90f), (rotateTime - 0.2f) / 0.5f);
            }
            if (rotateTime > 0.7f)
            {
                fireRotate = false;
            }
            rotateTime += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.K))
        {
            Debug.Log("P1");
            if (!p)
            {
                ro = transform.eulerAngles;
                p = true;
            }
            if (time < 0.5f)
            {
                //Debug.Log("P2");
                transform.eulerAngles = Vector3.Lerp(ro, ro + new Vector3(0, 0, 90f), time/0.5f);
            }
            if(time > 2f)
            {
                Debug.Log("P3");
                time = 0f;
                p = false;
            }
            time += Time.deltaTime;
            //transform.RotateAround(rotatePoint, new Vector3(1, 0, 0), 360 * Time.deltaTime * speed);
            //foreach (GameObject f in fires)
            //{
            //    f.transform.RotateAround( rotatePoint, new Vector3(1, 0, 0), 360 * Time.deltaTime * speed);
            //}
        }

    }
    private void Fire()
    {
        fireBalls.transform.GetChild(fireCount).gameObject.SetActive(false);
        GameObject fireBallObj = Instantiate(FireBallPrefab, fireBalls.transform.GetChild(fireCount).position, Quaternion.identity);
        fireBallObj.transform.forward = transform.forward;
        Destroy(fireBallObj, 0.5f);
        if (fireCount == 3)
        {
            fireCount = 0;
        }
    }
}
