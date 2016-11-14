using UnityEngine;
using System.Collections;

public class ClimbingHold : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("There was a collision!");
        gameObject.GetComponent<LineRenderer>().SetColors(UnityEngine.Color.green, UnityEngine.Color.green);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        gameObject.GetComponent<LineRenderer>().SetColors(UnityEngine.Color.red, UnityEngine.Color.red);
    }

}

