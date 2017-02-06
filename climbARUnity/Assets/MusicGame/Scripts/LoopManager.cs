using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoopManager : MonoBehaviour
{

    private AudioSource[] sources;
    public AudioClip[] clips;
    private Dictionary<int, int> soundMap;
    private bool[] activeSounds;
    private float SLEEP_TIME = 1.0f;

    // Use this for initialization
    void Start()
    {

    }

    public void Setup(string[] audioPaths)
    {
        soundMap = new Dictionary<int, int>();
        clips = new AudioClip[audioPaths.Length];
        sources = new AudioSource[audioPaths.Length];
        activeSounds = new bool[audioPaths.Length];
        for (int i = 0; i < audioPaths.Length; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            clips[i] = Resources.Load<AudioClip>(audioPaths[i]);
            activeSounds[i] = false;
        }
        StartCoroutine("StartTrackPlayback");
    }

    public void RegisterHold(int holdIndex, int soundId)
    {
        soundMap.Add(holdIndex, soundId);
    }

    public void Mute(int holdIndex)
    {
        if (soundMap.ContainsKey(holdIndex))
        {
            sources[holdIndex].volume = 0;
        }
    }

    public void Unmute(int holdIndex)
    {
        if (soundMap.ContainsKey(holdIndex))
        {
            sources[holdIndex].volume = 1;
        }
    }

    IEnumerator StartTrackPlayback()
    {

        for (int i = 0; i < activeSounds.Length; i++)
        {
            sources[i].PlayOneShot(clips[i], 0);
        }
        yield return new WaitForSeconds(clips[0].length);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
