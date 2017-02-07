using UnityEngine;
using UnityEngine.SceneManagement;

public class HoldSetup : MonoBehaviour
{

    void Update()
    {
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
        else if (Input.GetKeyDown("escape"))
        {
            if (Application.isEditor)
            {
                Debug.Log("Cannot quit the application (Application is editor).");
            }
            else
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

    }
}
