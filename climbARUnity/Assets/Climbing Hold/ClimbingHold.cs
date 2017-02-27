using UnityEngine;
using System.Collections;

public class ClimbingHold : MonoBehaviour
{

    public static Sprite customHoldSprite0;
    public static Sprite customHoldSprite1;
    private int enterCount;

    void Start()
    {
        enterCount = 0;
        customHoldSprite0 = Resources.Load<Sprite>("customHold0");
        customHoldSprite1 = Resources.Load<Sprite>("customHold1");
        if (customHoldSprite0 == null || customHoldSprite1 == null)
        {
            Debug.Log("Could not find both custom hold sprites necessary in Resources folder");
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
            if (gameObject.GetComponent<SpriteRenderer>().sprite != null)
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

    void OnTriggerEnter2D(Collider2D col)
    {
        enterCount++;
        if (enterCount > 0)
        {
            if (gameObject.GetComponent<SpriteRenderer>().sprite != null)
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
