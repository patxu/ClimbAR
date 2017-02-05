using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGame : MonoBehaviour {


    string[] soundItems = new string[1] { "Sounds/foghorn" }; //path relative to Resources folder
    GameObject[] holds;

	// Use this for initialization
	void Start () {

        holds = GameObject.FindGameObjectsWithTag("Hold");
        
        foreach (string soundItem in soundItems)
        {
            Debug.Log(soundItem);
            GameObject soundHold = holds[Random.Range(0, holds.Length)];
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
                soundHoldScript.Setup(soundItem);
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
