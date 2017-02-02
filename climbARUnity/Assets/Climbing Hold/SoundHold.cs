using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHold : ClimbingHold {
    public AudioClip audio;
    private AudioSource source;

    bool audioPlaying;

	// Use this for initialization
	void Start () {
        //Add the audiosource
        source = gameObject.AddComponent<AudioSource>();
        audioPlaying = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setup(string audioPath)
    {
        audio = Resources.Load<AudioClip>(audioPath);
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

    IEnumerator PlayAudioTrack()
    {
        source.PlayOneShot(audio);
        yield return new WaitForSeconds(audio.length);
    }
}
