using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{

    public StateManager stateManager;

    void Awake()
    {
        if (StateManager.instance == null)
        {
            Instantiate(stateManager);
        }
    }
}
