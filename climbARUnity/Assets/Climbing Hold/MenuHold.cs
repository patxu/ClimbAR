using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHold : SmoothedClimbingHold
{

    public string sceneName;
    private IEnumerator coroutine;
    private int enterCount;

    void OnStart()
    {
        enterCount = 0;
    }
    public GameObject canvasGameObject;

    // must call setup script
    public void setup(string sceneName)
    {
        this.sceneName = sceneName;
        coroutine = TransitionToSceneWithDelay(sceneName, 0.5f);
        TextMesh textMesh = gameObject.AddComponent<TextMesh>();
        textMesh.characterSize = 0.1f;
        textMesh.fontSize = 50;
        textMesh.text = SceneUtils.SceneNameToDisplayName[sceneName];
        textMesh.anchor = TextAnchor.MiddleLeft;
    }

    private new void OnTriggerExit2D(Collider2D col)
    {
        if (!ShouldRegisterHoldReleased(col))
        {
            return;
        }

        StopCoroutine(coroutine);

        gameObject.GetComponent<LineRenderer>()
            .startColor = UnityEngine.Color.cyan;
        gameObject.GetComponent<LineRenderer>()
            .endColor = UnityEngine.Color.cyan;

    }

    private new void OnTriggerEnter2D(Collider2D col)
    {
        if (!ShouldRegisterHoldGrabbed(col))
        {
            return;
        }

        StartCoroutine(coroutine);
        gameObject.GetComponent<LineRenderer>()
            .startColor = UnityEngine.Color.cyan;
        gameObject.GetComponent<LineRenderer>()
            .endColor = UnityEngine.Color.cyan;
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

    private void OnDisable()
    {
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        Destroy(textMesh);
    }
}
