using UnityEngine;
using System.Collections;

public class climbSystemEnv : MonoBehaviour {

    // return true if machine is Windows
    // important for things like choosing whether to import OpenCV
    public static bool isWindows()
    {
#if  UNITY_STANDALONE_WIN
        return true;
#else
        return false;
#endif
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
