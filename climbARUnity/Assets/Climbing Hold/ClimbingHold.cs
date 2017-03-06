using UnityEngine;
using System.Collections;
using System;


public class ClimbingHold : MonoBehaviour
{
    private int enterCount;
    private System.DateTime lastCountedCollision;
    private int smoothing = 750;
    public bool smoothingEnabled;


    void Start()
    {
        enterCount = 0;
        smoothingEnabled = true;
        lastCountedCollision = System.DateTime.UtcNow;
    }

    void Update()
    {
        
    }

    public bool ShouldRegisterHoldGrabbed(Collider2D col)
    {
        if (col != null && col.gameObject.tag == "Hold")
        {
            return false;
        }
        else if (smoothingEnabled)
        {
            enterCount++;
            System.DateTime currentTime = System.DateTime.UtcNow;
            TimeSpan diff = currentTime - lastCountedCollision;

            if (enterCount > 1 || diff.TotalMilliseconds < smoothing)
            {
                return false;
            }

            lastCountedCollision = System.DateTime.UtcNow;
            return true;
        }
        else
        {
            return true;
        }
    }

    public bool ShouldRegisterHoldReleased(Collider2D col)
    {
        if (col != null && col.gameObject.tag == "Hold")
        {
            return false;
        }
        else if (smoothingEnabled)
        {
            enterCount--;

            if (enterCount == 0)
            {
                lastCountedCollision = System.DateTime.UtcNow;
                return true;
            }
            else
            {
                return false;
            }
        }
        else 
        {
            return true;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (ShouldRegisterHoldReleased(col))
        {
            ClimbARHandhold.setHoldColor(gameObject, ClimbARHandhold.RESET_COLOR);
        }
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (ShouldRegisterHoldGrabbed(col))
        {
            ClimbARHandhold.setHoldColor(gameObject, ClimbARHandhold.ENTERED_COLOR);
        }
    }

    void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
