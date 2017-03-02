using UnityEngine;

public class RocMan : MonoBehaviour
{

    // Game Objects
    public GameObject[] ghosts;
    public GameObject ghost;
    public GameObject[] handholds;
    public GameObject Handhold;
    // TODO: include handholds when the game state requires access to them...
    // TODO: include skeleton tracking game objects...

    // Other variables 
    public const int numGhosts = 1;

    void Start()
    {
        this.CreateGhosts();
    }

    void Update()
    {

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
            this.ghosts[i].name = "Ghost " + i;
            this.ghosts[i].transform.localPosition = pos;
            this.ghosts[i].transform.localScale = scale;

            offset += 3;
        }
    }
}
