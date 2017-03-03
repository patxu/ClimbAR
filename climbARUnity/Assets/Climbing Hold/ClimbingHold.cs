using UnityEngine;
using System.Collections;
using System;
using System.Threading;


public class ClimbingHold : MonoBehaviour
{
    private int enterCount;
    private bool endStateKnown;
    private float grabbedSmoothing = 500 / 1000; // divided by 1000 to put as fraction of second
    private float releasedSmoothing = 500 / 1000;

    public bool smoothingEnabled;
    public enum States {Released, GrabbedPending, Grabbed, ReleasePending}
    public States currentState;

    private Guid id;


    void Start()
    {
        enterCount = 0;
        smoothingEnabled = true;
        currentState = States.Released;
        endStateKnown = true;
        id = Guid.NewGuid();
    }

    void Update()
    {
        
    }

    private IEnumerator grabbedPending()
    {
        endStateKnown = false;
        Debug.Log("About to sleep in state " + currentState.ToString());
        yield return new WaitForSeconds(0.1f);

        Debug.Log("Just woke up in state " + currentState.ToString() + " with enterCount: " + enterCount.ToString());
        // if after the sleep we are still in pending state, change to grabbed
        if (currentState == States.GrabbedPending)
        {
            currentState = States.Grabbed;
            endStateKnown = true;
        }
    }

    private IEnumerator releasedPending()
    {
        endStateKnown = false;

        yield return new WaitForSeconds(0.1f);

        // if after the sleep we are still in pending state, change to released
        if (currentState == States.ReleasePending) 
        {
            currentState = States.Released;
            endStateKnown = true;
        }
    }

    private void updateStateOnCollisionExit()
    {
        Debug.Log("hold " + id + " updated exit with state " + currentState.ToString() + " and enterCount " + enterCount.ToString());
        switch (currentState)
        {
            case States.Released:
                break;
            case States.GrabbedPending:
                if (enterCount == 0)
                {
                    Debug.Log("Whoops! hold " + id + " when grabbed pending comes back we will be released");
                    currentState = States.Released;
                }
                break;
            case States.Grabbed:
                if (enterCount == 0)
                {
                    currentState = States.ReleasePending;
                    StartCoroutine("releasedPending");
                }
                break;
            case States.ReleasePending:
                break;
        }
    }

    private void updateStateOnCollisionEnter()
    {
        Debug.Log("hold " + id + " updated enter with state " + currentState.ToString() + " and enterCount " + enterCount.ToString());
        switch (currentState)
        {
            case States.Released:
                currentState = States.GrabbedPending;
                Debug.Log("starting coroutine");
                StartCoroutine("grabbedPending");
                break;
            case States.GrabbedPending:
                break;
            case States.Grabbed:
                break;
            case States.ReleasePending:
                currentState = States.Grabbed;
                break;
        }
    }

    public bool ShouldRegisterHoldGrabbed(Collider2D col)
    {
        // Check that not colliding with overlapping hold
        if (col != null && col.gameObject.tag == "Hold")
        {
            return false;
        }
        
        // If we don't want smoothing, just return true
        if (!smoothingEnabled)
        {
            return true;
        }
        enterCount++;

        updateStateOnCollisionEnter();
        Debug.Log("Returning with state: " + currentState.ToString());

        if (endStateKnown)
        {
            return currentState == States.Grabbed;
        }
        else
        {
            Debug.Log("State not know");
            return false;
        }
    }

    public bool ShouldRegisterHoldReleased(Collider2D col)
    {
        // Check that not colliding with overlapping hold
        if (col != null && col.gameObject.tag == "Hold")
        {
            return false;
        }

        // If we don't want smoothing, just return true
        if (!smoothingEnabled)
        {
            return true;
        }
        enterCount--;
        updateStateOnCollisionExit();

        if (endStateKnown)
        {
            return currentState == States.Released;
        }
        else
        {
            Debug.Log("State not known");
            return false;
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
