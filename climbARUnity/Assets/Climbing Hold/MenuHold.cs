using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHold : ClimbingHold
{

    public string sceneName;
    private IEnumerator coroutine;
    private int enterCount;

    void OnStart()
    {
        enterCount = 0;
    }

    // must call setup script
    public void setup(string sceneName)
    {
        this.sceneName = sceneName;
        coroutine = TransitionToSceneWithDelay(sceneName, 2);

        TextMesh textMesh = gameObject.AddComponent<TextMesh>();
        //textMesh.font = Resources.Load<Font>("Fonts/CaviarDreams");
        textMesh.characterSize = 0.1f;
        textMesh.fontSize = 50;
        textMesh.text = SceneUtils.SceneNameToDisplayName[sceneName];
        textMesh.anchor = TextAnchor.MiddleLeft;
    }

    void OnUpdate()
    {

    }

    void OnTriggerExit2D(Collider2D col)
    {
        StopCoroutine(coroutine);
        enterCount--;
        if (enterCount == 0)
        {
            gameObject.GetComponent<LineRenderer>()
                .startColor = UnityEngine.Color.cyan;
            gameObject.GetComponent<LineRenderer>()
                .endColor = UnityEngine.Color.cyan;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        StartCoroutine(coroutine);
        enterCount++;
        if (enterCount > 0)
        {
            gameObject.GetComponent<LineRenderer>()
                .startColor = UnityEngine.Color.green;
            gameObject.GetComponent<LineRenderer>()
                .endColor = UnityEngine.Color.green;
        }
    }

    void OnMouseDown()
    {
        OnTriggerEnter2D(null);
    }

    IEnumerator TransitionToSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
}
