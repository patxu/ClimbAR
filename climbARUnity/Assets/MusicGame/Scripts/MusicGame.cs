using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGame : MonoBehaviour {

    string[] soundItems = new string[1] { "Sounds\\foghorn" };
    GameObject[] holds;

	// Use this for initialization
	void Start () {
        holds = GameObject.FindGameObjectsWithTag("Hold");
        
        foreach (string soundItem in soundItems)
        {
            GameObject soundHold = holds[Random.Range(0, holds.Length)];
            if (soundHold == null)
            {
                Debug.Log("no valid hold found");
            }
            else
            {
                ClimbingHold holdScript = soundHold.GetComponent<ClimbingHold>();
                Destroy(holdScript);

                SoundHold soundHoldScript = soundHold.AddComponent<SoundHold>();
                soundHoldScript.setup(soundItem);
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
