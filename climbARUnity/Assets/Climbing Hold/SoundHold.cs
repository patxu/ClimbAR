using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHold : ClimbingHold {
    public AudioClip audioClip;
    private AudioSource source;

    bool audioPlaying;

	// Use this for initialization
	void Start () {
        audioPlaying = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup(string audioPath)
    {
        audioClip = Resources.Load<AudioClip>(audioPath); //path relative to Resources folder
        source = this.gameObject.AddComponent<AudioSource>();
        source.clip = audioClip;
    }

    private void OnMouseDown()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (audioPlaying)
        {
            audioPlaying = false;
            source.Stop();
            StopCoroutine("PlayAudioTrack");
        }
        else
        {
            audioPlaying = true;
            StartCoroutine("PlayAudioTrack");
        }
    }

    void PlayAudioTrack()
    {
        this.source.volume = 4.0f;
        this.source.Play();
    }
}
