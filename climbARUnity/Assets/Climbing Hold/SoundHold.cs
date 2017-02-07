using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundHold : ClimbingHold
{
    public AudioClip audioClip;
    private AudioSource source;
    public LoopManager loopManager;
    public int holdIndex;

    bool audioPlaying;

    // Use this for initialization
    void Start()
    {
        audioPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setup(string audioPath, int holdIndex, LoopManager loopManager)
    {
        this.loopManager = loopManager;
        this.holdIndex = holdIndex;
        //audioClip = Resources.Load<AudioClip>(audioPath); //path relative to Resources folder
        //source = this.gameObject.AddComponent<AudioSource>();
        //source.clip = audioClip;
    }

    private void OnMouseDown()
    {
        OnTriggerEnter2D(null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(holdIndex);
        if (audioPlaying)
        {
            audioPlaying = false;
            gameObject.GetComponent<LineRenderer>()
                 .startColor = UnityEngine.Color.cyan;
            gameObject.GetComponent<LineRenderer>()
                .endColor = UnityEngine.Color.cyan;
            loopManager.Mute(holdIndex);
        }
        else
        {
            audioPlaying = true;
            loopManager.Unmute(holdIndex);
            gameObject.GetComponent<LineRenderer>()
               .startColor = UnityEngine.Color.red;
            gameObject.GetComponent<LineRenderer>()
                .endColor = UnityEngine.Color.red;
        }
    }
}
