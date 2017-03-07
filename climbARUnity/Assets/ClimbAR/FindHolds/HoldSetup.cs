using UnityEngine;
using UnityEngine.SceneManagement;

public class HoldSetup : MonoBehaviour
{

    private GameObject[] holds;
    public KinectClassify classifier;
    public bool autoClassify = true;
    public bool autoTransition = true;

    private void Start()
    {
        if (autoClassify)
        {
            if (classifier == null)
            {
                ClimbARUtils.LogError("No classifier specified for hold setup so cannot automatically classify and transition");
            }
            else
            {
                StateManager.instance.debugView = false;
                classifier.StartCoroutine(classifier.ClassifyImage);
            }

            if (autoTransition)
            {
                SceneManager.LoadScene(SceneUtils.SceneNames.menu);
            }
        }
    }

    void Update()
    {
        holds = GameObject.FindGameObjectsWithTag("Hold");

        if (Input.GetKeyDown("space"))
        {
            // don't move until we've flipped hold orientation - future scenes shouldn't have the live image
            if (StateManager.instance.debugView == true)
            {
                ClimbARUtils.LogError("Color view must be toggled off! Press <t>");
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
