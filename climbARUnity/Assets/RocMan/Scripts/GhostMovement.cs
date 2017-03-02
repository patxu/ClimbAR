using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GhostMovement : MonoBehaviour
{

    // State variables
    private float moveSpeed;
    private int lives;
    private bool offScreen;

    // Use this for initialization
    void Start()
    {
        this.moveSpeed = 0.5f;
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

    private void OnMouseDown()
    {
        OnTriggerEnter2D(null);
    }
}
