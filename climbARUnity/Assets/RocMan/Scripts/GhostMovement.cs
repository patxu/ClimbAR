using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GhostMovement : MonoBehaviour
{

    // State variables
    private float moveSpeed;
    private int lives;
    private bool offScreen;
    public GameObject livesRemaining;
    private GameObject childSpriteObject;

    // Use this for initialization
    void Start()
    {
        this.moveSpeed = 0.5f;
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.tag == "HoldSprite")
            {
                childSpriteObject = child.gameObject;
            }
        }
        this.lives = 10;
    }

    // Update is called once per frame
    void Update()
    {
        // keyboard controls
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.moveSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(this.moveSpeed, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-this.moveSpeed, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -this.moveSpeed);
        }

             // keep ghost on screen
        if (!childSpriteObject.GetComponent<SpriteRenderer>().isVisible)
        {
            if (offScreen == false) // was on screen before, now off, so reverse 
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * -1, GetComponent<Rigidbody2D>().velocity.y * -1);
            }
            offScreen = true;
        }
        else
        {
            offScreen = false;
        }

        // make ghost look left or right depending on x velocity
        // don't flip if == 0 so it doesn't change when moving up/down
        if (GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            childSpriteObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            childSpriteObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    void ReverseDirection()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * (-1), GetComponent<Rigidbody2D>().velocity.y * (-1));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        this.ReverseDirection();
        string layerName = LayerMask.LayerToName(col.gameObject.layer);
        switch (layerName)
        {
            case "Holds":
                // currently a nop
                break;
            case "Skeleton":
                this.lives--;
                break;
            default:
                break;
        }
    }

    void decrementLivesRemaining()
    {
        this.lives--;
        string[] strArray = livesRemaining.GetComponent<Text>().text.Split(" "[0]);
        foreach (var str in strArray)
        {
            int lives;
            bool success = int.TryParse(str, out lives);
            if (success)
            {
                lives--;
                break;
            }
        }

        livesRemaining.GetComponent<Text>().text = "Lives: " + lives;
    }

    private void OnMouseDown()
    {
        OnTriggerEnter2D(null);
    }
}
