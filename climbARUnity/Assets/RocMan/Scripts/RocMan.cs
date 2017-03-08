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

    // Other public variables 
    public TextMesh livesText;
    public TextMesh gameOverText;
    public TextMesh gameStartText;
    public const int numGhosts = 1;
    public int lives;
    public bool playing = false;

    // For in-game sound effects
    public AudioSource source;
    public static class RocManSounds 
    {
        public static string gameStart = "gameStart";
        public static string ghostCollision = "ghostCollision";
        public static string youDied = "youDied";
    }
    public static Dictionary<string, AudioClip> RocManSoundMap = new Dictionary<string, AudioClip>()
    {
        { RocManSounds.gameStart, null},
        { RocManSounds.ghostCollision, null},
        { RocManSounds.youDied, null},
    };

    // Private variables
    private IEnumerator coroutine;

    void Start()
    {
        // Set menu transition
        this.coroutine = TransitionToSceneWithDelay(SceneUtils.SceneNames.menu, 0.5f);

        // Set in-game text
        this.livesText.text = "Number of Lives: " + this.lives;
        this.livesText.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        this.gameOverText.text = "";
        this.gameOverText.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 3, Screen.height - Screen.height / 4, 0));

        this.gameStartText.text = "      Press m for menu scene\n        or space to start game";
        this.gameStartText.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 3, Screen.height, 0));

        // Set in-game sound effect
        this.source = gameObject.AddComponent<AudioSource>();
        RocManSoundMap[RocManSounds.gameStart] = Resources.Load<AudioClip>("RocManSounds/gameStart");
        RocManSoundMap[RocManSounds.ghostCollision] = Resources.Load<AudioClip>("RocManSounds/ghostCollision");
        RocManSoundMap[RocManSounds.youDied] = Resources.Load<AudioClip>("RocManSounds/youDied");

        // Reactivate line renderers for handholds
        this.handholds = GameObject.FindGameObjectsWithTag("Hold");
        for (int i = 0; i < handholds.Length; i++)
        {
            handholds[i].AddComponent<RocHold>();
            ClimbARHandhold.HoldLineRendererActive(handholds[i], true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            this.PauseMovement();
        }
        if (Input.GetKeyDown("m"))
        {
            this.TransitiontoMenuScene();
        }
        if (!playing)
        {
            if (Input.GetKeyDown("space"))
            {

                this.CleanupGhosts();
                this.StartGame();
            }
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
            col.radius = 4.0f;
            col.enabled = true;
            col.isTrigger = true;

            offset += 3;
        }
    }

    void CleanupGhosts()
    {
        for (int i = 0; i < RocMan.numGhosts; i++)
        {
            if (this.ghosts.Length != 0 && this.ghosts[i] != null)
            {
                Destroy(this.ghosts[i]);
            }
        }
    }

    void StartGame()
    {
        // Setup ghosts
        this.CreateGhosts();
        // Reset lives
        this.lives = 10;
        // Toggle on screen game text 
        this.gameOverText.text = "";
        this.gameStartText.text = "";
        this.livesText.text = "Number of Lives: " + this.lives;
        // Allow game state to resume
        this.playing = true;
        // Play game start sound
        AudioClip clip;
        RocManSoundMap.TryGetValue(RocManSounds.gameStart, out clip);
        source.PlayOneShot(clip);
    }


    IEnumerator TransitionToSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    void TransitiontoMenuScene()
    {
        Debug.Log("Going to menu scene");
        StartCoroutine(this.coroutine);
    }

    public void LoseLife()
    {
        this.lives--;
        if (this.lives > 0)
        {
            AudioClip clip;
            RocManSoundMap.TryGetValue(RocManSounds.ghostCollision, out clip);
            source.PlayOneShot(clip);
            this.livesText.text = "Number of Lives: " + this.lives;
        }
        else
        {
            this.ToggleEndGame();
        }
    }

    public void ToggleEndGame()
    {
        // Play game over sounds
        AudioClip clip;
        RocManSoundMap.TryGetValue(RocManSounds.youDied, out clip);
        source.PlayOneShot(clip);
        // Other end game stuff
        this.PauseMovement();
        this.livesText.text = "Number of Lives: 0";
        this.gameOverText.text = "                Game Over\n      Press m for menu scene\n      or space to restart game";
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

    private void OnDisable()
    {
        this.CleanupGhosts();
        foreach (GameObject hold in this.handholds)
        {
            if (hold != null)
            {
                RocHold s = hold.GetComponent<RocHold>();
                Destroy(s);
                ClimbARHandhold.HoldLineRendererActive(hold, false);
                ClimbARHandhold.DestroyChildren(hold);
            }
        }
    }
}