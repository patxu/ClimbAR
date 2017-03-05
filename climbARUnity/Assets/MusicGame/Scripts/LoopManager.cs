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
    private bool[] saveState;
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
        saveState = new bool[audioPaths.Length];
        for (int i = 0; i < audioPaths.Length; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            clips[i] = Resources.Load<AudioClip>(audioPaths[i]);
            activeSounds[i] = false;
        }
        StartCoroutine("StartTrackPlayback");
    }

    public void PauseSounds()
    {
        activeSounds.CopyTo(saveState, 0);
        
        // After saving state, mute all 
        foreach (KeyValuePair<int, int> entry in soundMap)
        {
            Mute(entry.Key);
        }
    }

    public void UnPauseSounds()
    {
        for (int i = 0; i < saveState.Length; i++)
        {
            if (saveState[i])
            {
                Unmute(i);
            }
        }
    }

    public void RegisterHold(int holdIndex, int soundId)
    {
        soundMap.Add(holdIndex, soundId);
    }

    public void Mute(int holdIndex)
    {
        if (soundMap.ContainsKey(holdIndex))
        {
            activeSounds[holdIndex] = false;
            sources[holdIndex].volume = 0;
        }
    }

    public void Unmute(int holdIndex)
    {
        if (soundMap.ContainsKey(holdIndex))
        {
            activeSounds[holdIndex] = true;
            sources[holdIndex].volume = 1;
        }
    }

    IEnumerator StartTrackPlayback()
    {
        bool firstTime = true;
        while (true)
        {
            for (int i = 0; i < activeSounds.Length; i++)
            {
                sources[i].PlayOneShot(clips[i]);
                if (firstTime)
                {
                    sources[i].volume = 0;
                }
            }
            firstTime = false;
            yield return new WaitForSeconds(clips[0].length);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
