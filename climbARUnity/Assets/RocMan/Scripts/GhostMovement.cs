using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostMovement : MonoBehaviour
{
    // State variables
    private float moveSpeed;
    public RocMan rocmanScript;
    public int xPos;
    public int yPos; 

    // Use this for initialization
    void Start()
    {
        this.moveSpeed = 0.5f;
        this.rocmanScript = Camera.main.GetComponent<RocMan>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.rocmanScript.playing)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(GetComponent<Transform>().position);
            this.xPos = (int) pos.x;
            this.yPos = (int) pos.y;
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
            // For Debug
            if (Input.GetKey("space"))
            {
                this.rocmanScript.ToggleEndGame();
            }
            // Avoid moving out of bounds
            if (xPos < 0 || xPos > Screen.width || yPos < 0 || yPos > Screen.height)
            {
                this.ReverseDirection();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        this.ReverseDirection();
        string layerName = LayerMask.LayerToName(col.gameObject.layer);
        switch (layerName)
        {
            case "Holds":
                break;
            case "Skeleton":
                this.rocmanScript.LoseLife();
                break;
            default:
                break;
        }
    }

    public void ReverseDirection()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * (-1), GetComponent<Rigidbody2D>().velocity.y * (-1));
    }
}
