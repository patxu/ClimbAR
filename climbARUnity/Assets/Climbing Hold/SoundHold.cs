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

    private void OnTriggerExit2D(Collider2D collision)
    {
        enterCount--;
        lastCountedCollision = System.DateTime.UtcNow;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enterCount++;

        // If there is already some hand joint inside circle, we dont want to count another joint entering as a new event
        if (enterCount > 1)
        {
            return;
        }

        System.DateTime currentTime = System.DateTime.UtcNow;
        TimeSpan diff = currentTime - lastCountedCollision;

        // If there has been a collision in the last second, ignore it
        if (diff.TotalMilliseconds < smoothing)
        {
            return;
        }

        // If it has been more than a second, update last collision time and switch audio
        lastCountedCollision = System.DateTime.UtcNow;
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
