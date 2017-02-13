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

    public delegate bool HoldAction(params object[] arguments);

    public bool HandHoldGrabbed(Collider2D col, HoldAction action, params object[] arguments)
    {
        if (col.gameObject.tag != "Hold")
        {
            return false;
        }
        if (action == null)
        {
            return true;
        }
        return action(arguments);
    }

    public bool HandHoldReleased(Collider2D col, HoldAction action, params object[] arguments)
    {
        if (col.gameObject.tag != "Hold")
        {
            return false;
        }
        if (action == null)
        {
            return true;
        }

        return action(arguments);
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        HandHoldReleased(col, null);

        /*
        enterCount--;
        if (enterCount == 0)
        {
            gameObject.GetComponent<LineRenderer>()
                .startColor = UnityEngine.Color.red;
            gameObject.GetComponent<LineRenderer>()
                .endColor = UnityEngine.Color.red;
        } */
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        HandHoldGrabbed(col, null);

        /*
        enterCount++;
        if (enterCount > 0)
        {
            gameObject.GetComponent<LineRenderer>()
                .startColor = UnityEngine.Color.green;
            gameObject.GetComponent<LineRenderer>()
                .endColor = UnityEngine.Color.green;
        }*/
    }

    void OnMouseDown()
    {
        Destroy(gameObject);
    }

    
}
