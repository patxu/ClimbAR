using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundHold : ClimbingHold {
    public AudioClip audioClip;
    private AudioSource source;
    public LoopManager loopManager;
    public Guid holdId;

    bool audioPlaying;

	// Use this for initialization
	void Start () {
        audioPlaying = false;
        

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup(string audioPath, LoopManager loopManager)
    {
        holdId = Guid.NewGuid();
        this.loopManager = loopManager;
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
        Debug.Log(holdId);
        if (audioPlaying)
        {
            audioPlaying = false;
            gameObject.GetComponent<LineRenderer>()
                 .startColor = UnityEngine.Color.cyan;
            gameObject.GetComponent<LineRenderer>()
                .endColor = UnityEngine.Color.cyan;
            loopManager.StopPlay(holdId);
        }
        else
        {
            audioPlaying = true;
            loopManager.StartPlay(holdId);
            gameObject.GetComponent<LineRenderer>()
               .startColor = UnityEngine.Color.red;
            gameObject.GetComponent<LineRenderer>()
                .endColor = UnityEngine.Color.red;
        }
    }
}
