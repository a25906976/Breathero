using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<GameObject> audios;

    private bool isWalk = false;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if ((OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp, OVRInput.Controller.LTouch) ||
              OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown, OVRInput.Controller.LTouch) ||
              OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.LTouch) ||
              OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.LTouch)))
        {
            if (!isWalk)
            {
                isWalk = true;
                audios[2].GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            audios[2].GetComponent<AudioSource>().Stop();
            isWalk = false;
        }
    }

    public void PlatStory(){
        audios[0].SetActive(true);
    }
    public void StopStory(){
        audios[0].SetActive(false);
    }
    public void FireBallAttact(){
        audios[1].GetComponent<AudioSource>().Play();
    }
    public void Walk(){
        audios[2].SetActive(true);
    }
    public void TreeAttact(){
        audios[3].GetComponent<AudioSource>().Play();
    }

    public void CountDownPlay()
    {
        audios[4].GetComponent<AudioSource>().Play();
    }

    public void CountDownStop()
    {
        audios[4].GetComponent<AudioSource>().Stop();
    }

    public void BoxInhalePlay()
    {
        audios[5].GetComponent<AudioSource>().Play();
    }

    public void BoxInhalePause()
    {
        audios[5].GetComponent<AudioSource>().Pause();
    }
    public void BoxInhaleUnpause()
    {
        audios[5].GetComponent<AudioSource>().UnPause();
    }

    public void BoxInhaleStop()
    {
        audios[5].GetComponent<AudioSource>().Stop();
    }

    public void BoxEndPlay()
    {
        audios[6].GetComponent<AudioSource>().Play();
    }
    public void BoxHoldPlay()
    {
        audios[7].GetComponent<AudioSource>().Play();
    }

    public void BoxHoldPause()
    {
        audios[7].GetComponent<AudioSource>().Pause();
    }
    public void BoxHoldUnpause()
    {
        audios[7].GetComponent<AudioSource>().UnPause();
    }

    public void BoxHoldStop()
    {
        audios[7].GetComponent<AudioSource>().Stop();
    }

    public void BossOpenPlay()
    {
        audios[8].GetComponent<AudioSource>().Play();
    }

    public void BossOpenStop()
    {
        audios[8].GetComponent<AudioSource>().Stop();
    }

    public void BossBattlePlay()
    {
        audios[9].GetComponent<AudioSource>().Play();
    }

    public void BossBattleStop()
    {
        audios[9].GetComponent<AudioSource>().Stop();
    }

    public void BossChange()
    {
        if (!audios[10].GetComponent<AudioSource>().isPlaying) { 
            audios[10].GetComponent<AudioSource>().Play(); 
        }
    }

    public void FireInhalePlay()
    {
        audios[11].GetComponent<AudioSource>().Play();
    }

    public void FireInhalePause()
    {
        audios[11].GetComponent<AudioSource>().Pause();
    }
    public void FireInhaleUnpause()
    {
        audios[11].GetComponent<AudioSource>().UnPause();
    }

    public void FireInhaleStop()
    {
        audios[11].GetComponent<AudioSource>().Stop();
    }
    public void Ending()
    {
        audios[12].GetComponent<AudioSource>().Play();
    }
}
