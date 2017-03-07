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

    private bool audioPlaying;

    private States displayedState;

    // Use this for initialization
    void Start()
    {
        audioPlaying = false;
        displayedState = States.Released;
    }

    void Update()
    {
        if (displayedState == States.Released && base.currentState == States.Grabbed)
        {
            displayedState = States.Grabbed;
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
        else if (displayedState == States.Grabbed && base.currentState == States.Released)
        {
            displayedState = States.Released;
        }
    }

    public void Setup(string audioPath, int holdIndex, LoopManager loopManager)
    {
        lastCountedCollision = System.DateTime.UtcNow;
        this.loopManager = loopManager;
        this.holdIndex = holdIndex;
    }

    private void OnMouseDown()
    {
       enterCount = 0;
       OnTriggerEnter2D(null); 
    }

    private void OnDisable()
    {
        TextMesh textMesh = gameObject.GetComponentInChildren<TextMesh>();
        Destroy(textMesh);
    }
}
