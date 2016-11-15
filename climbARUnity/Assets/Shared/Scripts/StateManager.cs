using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour
{

    // Variables relating to state
    public Vector2 kinectUpperLeft, kinectUpperRight, kinectLowerLeft, kinectLowerRight;

    public static StateManager instance = null;
    void Awake()
    {
        Debug.Log("in awake for game manager");
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
