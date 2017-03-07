using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHold : ClimbingHold
{

    private int enterCount;
    private IEnumerator coroutine;
    public string sceneName;

    void Start()
    {
        enterCount = 0;
    }
    public GameObject canvasGameObject;

    // must call setup script
    public void setup(string sceneName)
    {
        this.sceneName = sceneName;
        coroutine = TransitionToSceneWithDelay(sceneName, 0.5f);
    }

    private new void OnTriggerExit2D(Collider2D col)
    {
        if (!ShouldRegisterHoldReleased(col))
        {
            return;
        }

        StopCoroutine(coroutine);

        if (gameObject.GetComponent<SpriteRenderer>().sprite != null)
        {
            Sprite currSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            gameObject.GetComponent<SpriteRenderer>().sprite = (currSprite == Menu.customHoldSprite0)
                ? Menu.customHoldSprite1
                : Menu.customHoldSprite0;
        }
        else
        {
            ClimbARHandhold.setHoldColor(gameObject, UnityEngine.Color.cyan);
        }
    }

    private new void OnTriggerEnter2D(Collider2D col)
    {
        if (!ShouldRegisterHoldGrabbed(col))
        {
            return;
        }

        StartCoroutine(coroutine);

        if (gameObject.GetComponent<SpriteRenderer>().sprite != null)
        {
            Sprite currSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            gameObject.GetComponent<SpriteRenderer>().sprite = (currSprite == Menu.customHoldSprite0)
                ? Menu.customHoldSprite1
                : Menu.customHoldSprite0;
        }
        else
        {
            ClimbARHandhold.setHoldColor(gameObject, UnityEngine.Color.cyan);
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

    private void OnDisable()
    {
        // Destroy menu text mesh and reset line renderers to uniform color
        TextMesh textMesh = gameObject.GetComponentInChildren<TextMesh>();
        Destroy(textMesh);
    }
}
