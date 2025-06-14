using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class music_set : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip story;
    public AudioClip fight;
    AudioSource audiosource;

    void Start()
    {
        audiosource = GameObject.FindGameObjectWithTag ("Background Music").GetComponent<AudioSource> ();
        audiosource.PlayOneShot(story);
    }
}
