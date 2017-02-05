using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MusicGame : MonoBehaviour {

    bool DEBUG = false;
    public GameObject prefabHold;

    string[] soundItems = new string[1] { "Sounds/foghorn" }; //path relative to Resources folder
    GameObject[] holds;

	// Use this for initialization
	void Start () {

        holds = GameObject.FindGameObjectsWithTag("Hold");


        // If starting directly into music scene, holds will be empty
        if (DEBUG)
        {
            holds = ClimbARHandhold.InstantiateHandholds(prefabHold, GetComponent<Camera>(), new float[] { 1f, 1f, 0.5f, 0.5f });
            holds[0].transform.localPosition = new Vector2(0, 0);
        }
        // If not debugging and no holds, just return
        else if (holds.Length <= 0)
        {
            return;
        }

        // Otherwise we actually have holds, assign it to hold
        HashSet<int> usedIndexes = new HashSet<int>();
        int index;

        for (int i = 0; i < Mathf.Min(holds.Length, soundItems.Length); i++) { 

            // Make sure we don't assign a hold multiple sounds
            do
            {
                index = UnityEngine.Random.Range(0, holds.Length);
            } while (usedIndexes.Contains(index));
            usedIndexes.Add(index);

            GameObject soundHold = holds[index];
            if (soundHold == null)
            {
                Debug.Log("no valid hold found");
            }
            else
            {
                Debug.Log("init sound hold");
                ClimbingHold holdScript = soundHold.GetComponent<ClimbingHold>();
                Destroy(holdScript);

                SoundHold soundHoldScript = soundHold.AddComponent<SoundHold>();
                soundHoldScript.Setup(soundItems[i]);
                soundHoldScript.GetComponent<LineRenderer>()
                    .startColor = UnityEngine.Color.cyan;
                soundHoldScript.GetComponent<LineRenderer>()
                    .endColor = UnityEngine.Color.cyan;
            }
        }	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
