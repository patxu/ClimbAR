using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MusicGame : MonoBehaviour {

    bool DEBUG = true;
    public GameObject prefabHold;

    string[] soundItems = new string[] { "Sounds/snare1", "Sounds/kick1", "Sounds/hihat1", "Sounds/crash1"}; //path relative to Resources folder
    GameObject[] holds;

	// Use this for initialization
	void Start () {

        holds = GameObject.FindGameObjectsWithTag("Hold");
        LoopManager loopManager = gameObject.AddComponent<LoopManager>();
        loopManager.Setup(soundItems);

        // If starting directly into music scene, holds will be empty
        if (DEBUG)
        {
            holds = ClimbARHandhold.InstantiateHandholds(prefabHold, GetComponent<Camera>(), new float[] { 1f, 1f, 0.2f, 0.2f , 2f, 2f, 0.2f, 0.2f, 1f, 1f, 0.1f, 0.1f});
            holds[0].transform.localPosition = new Vector2(-1, -1);
            holds[1].transform.localPosition = new Vector2(1, 1);
            holds[2].transform.localPosition = new Vector2(-1, 1);
        }
        // If not debugging and no holds, just return
        else if (holds.Length <= 0)
        {
            return;
        }

        // Otherwise we actually have holds, assign it to hold
        HashSet<int> usedHoldIndexes = new HashSet<int>();
        HashSet<int> usedSoundIndexes = new HashSet<int>();

        int soundId;
        for (int i = 0; i < Mathf.Min(holds.Length, soundItems.Length); i++) { 
            GameObject soundHold = holds[getUniqueRandom(usedHoldIndexes, holds.Length)];
            if (soundHold == null)
            {
                Debug.Log("no valid hold found");
            }
            else
            {
                ClimbingHold holdScript = soundHold.GetComponent<ClimbingHold>();
                Destroy(holdScript);

                SoundHold soundHoldScript = soundHold.AddComponent<SoundHold>();

                soundId = getUniqueRandom(usedSoundIndexes, soundItems.Length);

                soundHoldScript.Setup(soundItems[soundId], loopManager);

                loopManager.RegisterHold(soundHoldScript.holdId, soundId);

                soundHoldScript.GetComponent<LineRenderer>()
                    .startColor = UnityEngine.Color.cyan;
                soundHoldScript.GetComponent<LineRenderer>()
                    .endColor = UnityEngine.Color.cyan;
            }
        }	
	}

    private int getUniqueRandom(HashSet<int> used, int maxExclusive)
    {
        int index;
        do
        {
            index = UnityEngine.Random.Range(0, maxExclusive);
        } while (used.Contains(index));
        used.Add(index);
        return index;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
