using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocMan : MonoBehaviour
{

    // Game Objects
    public Camera mainCam;
    public GameObject[] ghosts;
    public GameObject ghost;
    public GameObject[] handholds;
    public GameObject Handhold;

    // Other variables 
    public TextMesh livesText;
    public TextMesh gameOverText;
    public const int numGhosts = 1;
    public int lives = 10;
    public bool playing = false;

    void Start()
    {
        this.CreateGhosts();
        this.livesText.text = "Number of Lives: " + this.lives;
        this.livesText.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 10));
        this.gameOverText.text = "";
        this.gameOverText.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 3, Screen.height - Screen.height/4, 5));
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            this.PauseMovement();
        }
    }

    void CreateGhosts()
    {
        int startX = 0; int startY = 0;
        int offset = 0;
        this.ghosts = new GameObject[RocMan.numGhosts];
        for (int i = 0; i < RocMan.numGhosts; i++)
        {
            Vector2 pos = new Vector2(startX + offset, startY);
            Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);

            this.ghosts[i] = GameObject.Instantiate(ghost);
            this.ghosts[i].transform.localPosition = pos;
            this.ghosts[i].transform.localScale = scale;

            Rigidbody2D rigid = this.ghosts[i].AddComponent<Rigidbody2D>();
            rigid.isKinematic = true;

            CircleCollider2D col = this.ghosts[i].AddComponent<CircleCollider2D>();
            col.radius = 0.1f;
            col.enabled = true;
            col.isTrigger = true;

            offset += 3;
        }
    }

    public void LoseLife()
    {
        this.lives--;
        if (this.lives > 0)
        {
            this.livesText.text = "Number of Lives: " + this.lives;
        }
        else
        {
            this.ToggleEndGame();
        }
    }

    public void ToggleEndGame()
    {
        this.PauseMovement();
        this.gameOverText.text = "                Game Over\n      Press m for menu scene\n           or r to restart game";
        this.playing = false;

    }

    public void RestartMovement()
    {
        int xDir = (Random.Range(0, 2) == 1) ? 1 : -1;
        int yDir = (Random.Range(0, 2) == 1) ? 1 : -1;
        for (int i = 0; i < RocMan.numGhosts; i++)
        {
            if (this.ghosts[i].GetComponent<Rigidbody2D>() != null)
            {
                this.ghosts[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0.5f * xDir, 0.5f * yDir);
            }
        }
    }

    public void PauseMovement()
    {
        for (int i = 0; i < RocMan.numGhosts; i++)
        {
            if (this.ghosts[i].GetComponent<Rigidbody2D>() != null)
            {
                this.ghosts[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }
    }
}
