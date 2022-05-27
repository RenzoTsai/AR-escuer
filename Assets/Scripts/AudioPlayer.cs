using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource m_MyAudioSource;
    public AudioSource audioSource;
    public float startTime;
    public bool hasPlayed;
    public int delay = 5;
    private float remainTime = 5;

    void Start()
    {
    	hasPlayed = false;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        remainTime = Time.time - startTime;
        if ((Time.time - startTime > delay) && !hasPlayed){
            audioSource.Play();
            hasPlayed = true;
        }
        

        
        
    }
}
