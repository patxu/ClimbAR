using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SmoothedClimbingHold : ClimbingHold {

    private System.DateTime lastCountedCollision;
    private int smoothing = 750;
    private int enterCount = 0;

    // Use this for initialization
    void Start () {
        lastCountedCollision = System.DateTime.UtcNow;
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public new bool ShouldRegisterHoldReleased(Collider2D col)
    { 
        if (!base.ShouldRegisterHoldReleased(col))
        {
            return false;
        }

        enterCount--;
        lastCountedCollision = System.DateTime.UtcNow;

        return true;
    }

    public new bool ShouldRegisterHoldGrabbed(Collider2D col)
    {
        if (!base.ShouldRegisterHoldGrabbed(col))
        {
            return false;
        }

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

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        ShouldRegisterHoldGrabbed(collision);
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        ShouldRegisterHoldReleased(collision);
    }

    
}
