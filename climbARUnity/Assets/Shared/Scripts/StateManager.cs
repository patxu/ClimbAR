using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour
{

    // Variables relating to state
    public int kinectUpperLeftX, kinectUpperLeftY, kinectWidth, kinectHeight;

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

    // Update is called once per frame
    void Update()
    {

    }
}
