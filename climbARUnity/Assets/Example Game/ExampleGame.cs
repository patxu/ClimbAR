using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleGame : MonoBehaviour {

    GameObject[] holds;
    public GameObject prefabHold;
    GameObject backHold;

	// Use this for initialization
	void Start () {
        holds = GameObject.FindGameObjectsWithTag("Hold");
        ClimbARHandhold.setHoldActivated(backHold, true); 

        //if (holds.Length == 0)
        //{
        //    holds = ClimbARHandhold.InstantiateHandholds(prefabHold, GetComponent<Camera>(), new float[] { 1f, 1f, 0.1f, 0.1f });
        //    holds[0].transform.localPosition = new Vector2(-1, -1);
        //    DontDestroyOnLoad(holds[0]);
        //    ClimbingHold script = holds[0].GetComponent<ClimbingHold>();
        //    Destroy(script);
        //}

        //backHold = holds[0];
        //MenuHold menuHoldScript = backHold.AddComponent<MenuHold>();
        //menuHoldScript.setup(SceneUtils.SceneNames.menu);
        //backHold.GetComponent<LineRenderer>()
        //    .startColor = UnityEngine.Color.cyan;
        //backHold.GetComponent<LineRenderer>()
        //    .endColor = UnityEngine.Color.cyan;

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnDisable()
    {
        MenuHold script = backHold.GetComponent<MenuHold>();
        Destroy(script);

        ClimbARHandhold.setHoldActivated(backHold, false);
    }
}
