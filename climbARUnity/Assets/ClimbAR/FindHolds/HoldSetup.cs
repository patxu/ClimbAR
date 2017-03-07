using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HoldSetup : MonoBehaviour
{

    private GameObject[] holds;
    public KinectClassify classifier;
    public bool autoClassify = true;
    public bool autoTransition = true;
    private float delay = 5f;

    private void Start()
    {
        if (autoClassify)
        {
            if (classifier == null)
            {
                Debug.LogError("No classifier specified for hold setup so cannot automatically classify and transition");
            }
            else
            {
                StateManager.instance.debugView = false;
                classifier.StartCoroutine(classifier.ClassifyImage, delay);
            }

            if (autoTransition)
            {
                StartCoroutine(loadNextScene());
            }
        }
    }

    IEnumerator loadNextScene()
    {
        yield return new WaitForSeconds(delay + 0.2f);
        SceneManager.LoadScene(SceneUtils.SceneNames.menu);
    }

    void Update()
    {
        holds = GameObject.FindGameObjectsWithTag("Hold");

        if (Input.GetKeyDown("space"))
        {
            // don't move until we've flipped hold orientation - future scenes shouldn't have the live image
            if (StateManager.instance.debugView == true)
            {
                Debug.LogError("Color view must be toggled off! Press <t>");
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
            if (hold != null)
            {
                ClimbingHold script = hold.GetComponent<ClimbingHold>();
                Destroy(script);
                ClimbARHandhold.HoldLineRendererActive(hold, false);
            }
        }
    }
}
