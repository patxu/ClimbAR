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
        this.lives = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, this.moveSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(this.moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-this.moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -this.moveSpeed);
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
        if (this.lives <= 0)
        {
           SceneManager.LoadScene(SceneUtils.SceneNames.rocManYouDied);
        }
    }
}
