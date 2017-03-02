using UnityEngine;
using UnityEngine.SceneManagement;

public class HoldSetup : MonoBehaviour
{

    GameObject[] holds;

    private void Start()
    {
    }

    void Update()
    {
        holds = GameObject.FindGameObjectsWithTag("Hold");

        if (Input.GetKeyDown("space"))
        {
            // don't move until we've flipped hold orientation - future scenes shouldn't have the live image
            if (StateManager.instance.debugView == true)
            {
                Debug.Log("Color view must be toggled off! Press <t>");
            }
            else
            {
                SceneManager.LoadScene(SceneUtils.SceneNames.menu);
            }
        }
    }

    private void OnDisable()
    {
        foreach (GameObject hold in holds)
        {
            ClimbingHold script = hold.GetComponent<ClimbingHold>();
            Destroy(script);
        }
    }
}
