using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SmoothedClimbingHold : ClimbingHold {

    private System.DateTime lastCountedCollision;
    private int smoothing = 1000;
    private int enterCount = 0;

    // Use this for initialization
    void Start () {
        lastCountedCollision = System.DateTime.UtcNow;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public new bool ShouldTriggerExit2D(Collider2D col)
    {
        return base.ShouldTriggerExit2D(col);
    }

    public new bool ShouldTriggerEnter2D(Collider2D col)
    {
        System.DateTime currentTime = System.DateTime.UtcNow;
        TimeSpan diff = currentTime - lastCountedCollision;

        return (base.ShouldTriggerEnter2D(col) && enterCount <= 1 && diff.TotalMilliseconds < smoothing);
    }


    public bool HoldReleased(params object[] args)
    {
        enterCount--;
        lastCountedCollision = System.DateTime.UtcNow;
        return true;
    }

    public new void OnTriggerExit2D(Collider2D col)
    {
        HoldAction action = HoldReleased;
        HandHoldReleased(col, action);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        enterCount++;
        base.OnTriggerEnter2D(col);
    }
}
