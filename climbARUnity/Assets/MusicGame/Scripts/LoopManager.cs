using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoopManager : MonoBehaviour {

    private AudioSource[] sources;
    public AudioClip[] clips;
    private Dictionary<Guid, int> soundMap;
    private bool[] activeSounds;
    private float SLEEP_TIME = 1.0f;

	// Use this for initialization
	void Start () {
        
	}

    public void Setup(string[] audioPaths)
    {
        soundMap = new Dictionary<Guid, int>();
        clips = new AudioClip[audioPaths.Length];
        sources = new AudioSource[audioPaths.Length];
        activeSounds = new bool[audioPaths.Length];
        for (int i = 0; i < audioPaths.Length; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            clips[i] = Resources.Load<AudioClip>(audioPaths[i]);
            activeSounds[i] = false;
        }
        StartCoroutine("PlayLoop");
    }

    public void RegisterHold(Guid holdId, int soundId)
    {
        Debug.Log(holdId);
        soundMap.Add(holdId, soundId);
    }

    public void StartPlay(Guid holdGuid)
    {
        Debug.Log(holdGuid.ToString());
        if (soundMap.ContainsKey(holdGuid))
        {
            activeSounds[soundMap[holdGuid]] = true;
        } else
        {
            Debug.Log("not in dict");
        }
    } 

    public void StopPlay(Guid holdGuid)
    {
        if (soundMap.ContainsKey(holdGuid))
        {
            activeSounds[soundMap[holdGuid]] = false;
        }
    }

    IEnumerator PlayLoop ()
    {
        while (true)
        {
            for (int i = 0; i < activeSounds.Length; i++)
            {
                if (activeSounds[i])
                {
                    sources[i].PlayOneShot(clips[i]);
                }
            }
            yield return new WaitForSeconds(SLEEP_TIME);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
