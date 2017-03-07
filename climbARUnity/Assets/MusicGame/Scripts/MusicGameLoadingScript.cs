using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicGameLoadingScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneManager.LoadSceneAsync(SceneUtils.SceneNames.musicGame);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
