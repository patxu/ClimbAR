using UnityEngine;
using System.Collections;
using System;


public class ClimbingHold : MonoBehaviour
{
  
    private int enterCount;

    void OnStart()
    {
        enterCount = 0;
    }

    void OnUpdate()
    {

    }

    public delegate bool HoldAction();

    public bool ShouldRegisterHoldGrabbed(Collider2D col)
    {
        if (col.gameObject.tag != "Hold")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ShouldRegisterHoldReleased(Collider2D col)
    {
        if (col.gameObject.tag != "Hold")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        gameObject.GetComponent<LineRenderer>()
            .startColor = UnityEngine.Color.red;
        gameObject.GetComponent<LineRenderer>()
            .endColor = UnityEngine.Color.red;
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        gameObject.GetComponent<LineRenderer>()
            .startColor = UnityEngine.Color.green;
        gameObject.GetComponent<LineRenderer>()
            .endColor = UnityEngine.Color.green;

    }

    void OnMouseDown()
    {
        Destroy(gameObject);
    }

    
}
