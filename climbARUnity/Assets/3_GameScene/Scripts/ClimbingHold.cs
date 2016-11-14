using UnityEngine;
using System.Collections;

public class ClimbingHold : MonoBehaviour {
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("There was a collision!");
    }
}

