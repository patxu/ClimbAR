using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundHold : SmoothedClimbingHold
{
    public AudioClip audioClip;
    private AudioSource source;
    public LoopManager loopManager;
    public int holdIndex;

    private System.DateTime lastCountedCollision;
    private int smoothing = 1000;
    private int enterCount = 0;

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
        lastCountedCollision = System.DateTime.UtcNow;
        this.loopManager = loopManager;
        this.holdIndex = holdIndex;
        //audioClip = Resources.Load<AudioClip>(audioPath); //path relative to Resources folder
        //source = this.gameObject.AddComponent<AudioSource>();
        //source.clip = audioClip;
    }

    private void OnMouseDown()
    {
       enterCount = 0;
       OnTriggerEnter2D(null); 

    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ShouldRegisterHoldGrabbed(collision))
        {
            return;
        }

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
               .startColor = UnityEngine.Color.green;
            gameObject.GetComponent<LineRenderer>()
                .endColor = UnityEngine.Color.green;
        }

    }
}
