using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GhostMovement : MonoBehaviour
{

    // State variables
    private float moveSpeed;
    public RocMan rocmanScript;

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
                break;
            case "Skeleton":
                this.rocmanScript.LoseLife();
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
