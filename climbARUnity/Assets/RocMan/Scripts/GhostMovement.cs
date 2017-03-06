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

    // Use this for initialization
    void Start()
    {
        this.moveSpeed = 0.5f;
        this.lives = 3;
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
        if (!GetComponent<SpriteRenderer>().isVisible)
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
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // reverse velocity
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * -1, GetComponent<Rigidbody2D>().velocity.y * -1);

        if (col != null && col.gameObject.tag == "Hold")
        {
        }
        else
        {
            decrementLivesRemaining();
        }

        if (this.lives <= 0)
        {
            SceneManager.LoadScene(SceneUtils.SceneNames.rocManYouDied);
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
