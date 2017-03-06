using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostMovement : MonoBehaviour
{

    // State variables
    private float moveSpeed;
    private int lives;

    // Use this for initialization
    void Start()
    {
        this.moveSpeed = 0.5f;
        this.lives = 3;
    }

    // Update is called once per frame
    void Update()
    {
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

        if (!GetComponent<SpriteRenderer>().isVisible)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * -1, GetComponent<Rigidbody2D>().velocity.y * -1);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * -1, GetComponent<Rigidbody2D>().velocity.y * -1);
        this.lives--;
        Debug.Log("You now have " + this.lives + " lives.");
        if (this.lives <= 0)
        {
            SceneManager.LoadScene(SceneUtils.SceneNames.rocManYouDied);
        }
    }
}
