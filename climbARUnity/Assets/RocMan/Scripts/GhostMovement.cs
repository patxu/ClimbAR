using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GhostMovement : MonoBehaviour
{
    // State variables
    private float moveSpeed;
    public RocMan rocmanScript;
    public int xPos;
    public int yPos;
    private System.DateTime lastCountedCollision;
    private int smoothingTime = 500; // time in milliseconds before we count the next collision

    // Use this for initialization
    void Start()
    {
        lastCountedCollision = System.DateTime.UtcNow;
        this.moveSpeed = .6f;
        this.rocmanScript = Camera.main.GetComponent<RocMan>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.rocmanScript.playing)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(GetComponent<Transform>().position);
            this.xPos = (int)pos.x;
            this.yPos = (int)pos.y;
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
            // For Debug
            if (Input.GetKey("d"))
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
            string layerName = LayerMask.LayerToName(col.gameObject.layer);
            switch (layerName)
            {
                case "Holds":
                    this.ReverseDirection();
                    Debug.Log("Reverse Direction");
                    break;
                case "Skeleton":
                    System.DateTime currentTime = System.DateTime.UtcNow;
                    TimeSpan diff = currentTime - lastCountedCollision;
                    if (diff.TotalMilliseconds > smoothingTime)
                    {
                        this.ReverseDirection();
                        this.rocmanScript.LoseLife();
                        lastCountedCollision = System.DateTime.UtcNow;
                    }
                    break;
                default:
                    break;
            }
            lastCountedCollision = System.DateTime.UtcNow;
    }


    public void ReverseDirection()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * (-1), GetComponent<Rigidbody2D>().velocity.y * (-1));
        gameObject.transform.position = new Vector2(gameObject.transform.position.x + (GetComponent<Rigidbody2D>().velocity.x/10),
                                                    gameObject.transform.position.y + (GetComponent<Rigidbody2D>().velocity.y/10));
    }
}