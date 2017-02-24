using UnityEngine;
using System.Collections;
using System;

public class ClimbingHold : MonoBehaviour
{
    public static Sprite customHoldSprite0;
    public static Sprite customHoldSprite1;
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
        customHoldSprite0 = Resources.Load<Sprite>("customHold0");
        customHoldSprite1 = Resources.Load<Sprite>("customHold1");
        if (customHoldSprite0 == null || customHoldSprite1 == null)
        {
            Debug.Log("Could not find both custom hold sprites necessary in Resources folder");
        }

        if (col != null && col.gameObject.tag == "Hold")
        {
            return false;
        }
        else if (smoothingEnabled)
        {
            enterCount--;
            lastCountedCollision = System.DateTime.UtcNow;

            return true;
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
            if (gameObject.GetComponent<SpriteRenderer>() != null)
            {
                Sprite currSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                gameObject.GetComponent<SpriteRenderer>().sprite = (currSprite == ClimbingHold.customHoldSprite0)
                    ? ClimbingHold.customHoldSprite1
                    : ClimbingHold.customHoldSprite0;
            }
            else
            {
                gameObject.GetComponent<LineRenderer>()
                  .startColor = UnityEngine.Color.red;
                gameObject.GetComponent<LineRenderer>()
                  .endColor = UnityEngine.Color.red;
 
            }
       }
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (ShouldRegisterHoldGrabbed(col))
        {
            if (gameObject.GetComponent<SpriteRenderer>() != null)
            {
                Sprite currSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                gameObject.GetComponent<SpriteRenderer>().sprite = (currSprite == ClimbingHold.customHoldSprite0)
                    ? ClimbingHold.customHoldSprite1
                    : ClimbingHold.customHoldSprite0;
            }
            else
            {
                gameObject.GetComponent<LineRenderer>()
                  .startColor = UnityEngine.Color.green;
                gameObject.GetComponent<LineRenderer>()
                  .endColor = UnityEngine.Color.green;
            }
       }
    }

    void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
