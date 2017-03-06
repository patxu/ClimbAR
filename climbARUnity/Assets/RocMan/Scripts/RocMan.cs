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
    public GameObject ghostSprite;
    //public GameObject[] handholds;
    //public GameObject Handhold;
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
            this.ghosts[i].transform.localPosition = pos;
            this.ghosts[i].transform.localScale = scale;

            GameObject ghostSpriteObject = GameObject.Instantiate(ghostSprite);
            ghostSpriteObject.transform.localPosition = pos;
            ghostSpriteObject.transform.localScale = scale;
            ghostSpriteObject.transform.SetParent(this.ghosts[i].transform);

            offset += 3;
        }
        // TODO: in GhostMovement script add movement and collision detection -- and upon collision, migrate to new scene: https://docs.unity3d.com/Manual/CollidersOverview.html
        // TODO: after collision stuff, add skeleton stuff
        // TODO: after skeleton collision, brainstorm mini game to build for RocMan
    }
}
