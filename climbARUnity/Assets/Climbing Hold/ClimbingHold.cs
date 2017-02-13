using UnityEngine;
using System.Collections;

public class ClimbingHold : MonoBehaviour
{
    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    Debug.Log("There was a collision!");
    //    gameObject.GetComponent<LineRenderer>().SetColors(UnityEngine.Color.green, UnityEngine.Color.green);
    //}

    public static Sprite customHoldSprite;
    private int enterCount;

    void Start()
    {
        enterCount = 0;
        customHoldSprite = Resources.Load<Sprite>("customHold");
        if (customHoldSprite == null)
        {
            Debug.Log("No custom hold found in resources folder");
        }
    }

    void Update()
    {

    }

    void OnTriggerExit2D(Collider2D col)
    {
        enterCount--;
        if (enterCount == 0)
        {
            gameObject.GetComponent<LineRenderer>()
                .startColor = UnityEngine.Color.red;
            gameObject.GetComponent<LineRenderer>()
                .endColor = UnityEngine.Color.red;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        enterCount++;
        if (enterCount > 0)
        {
            gameObject.GetComponent<LineRenderer>()
                .startColor = UnityEngine.Color.green;
            gameObject.GetComponent<LineRenderer>()
                .endColor = UnityEngine.Color.green;
        }
    }

    void OnMouseDown()
    {
        Destroy(gameObject);
    }

}
