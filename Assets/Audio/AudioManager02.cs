using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager02 : MonoBehaviour
{
    public List<GameObject> audio02;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayStory(){
        audio02[0].SetActive(true);
    }
    public void StopStory(){
        audio02[0].SetActive(false);
    }
    public void Rock(){
        audio02[1].SetActive(true);
    }
    public void CreateBox(){
        audio02[2].SetActive(true);
    }
    public void HaltBox(){
        audio02[3].SetActive(true);
    }
    public void FinishBox(){
        audio02[4].SetActive(true);
    }
}
