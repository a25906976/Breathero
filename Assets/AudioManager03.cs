using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager03 : MonoBehaviour
{
    public List<GameObject> audios;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayStory(){
        audios[0].SetActive(true);
    }
    public void StopStory(){
        audios[0].SetActive(false);
    }
    public void FireAttact(){
        audios[1].SetActive(true);
    }
    
}
