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
    public GameObject canvasGameObject;

    // must call setup script
    public void setup(string sceneName)
    {
        this.sceneName = sceneName;
        coroutine = TransitionToSceneWithDelay(sceneName, 1);

        //canvasGameObject = new GameObject();
        //canvasGameObject.name = "MenuCanvas:" + sceneName;
        //canvasGameObject.AddComponent<Canvas>();
        //Canvas canvas = canvasGameObject.GetComponent<Canvas>();
        //canvas.renderMode = RenderMode.ScreenSpaceOverlay; // ?
        //canvasGameObject.AddComponent<CurvedText>();
        //CurvedText textComponent = canvasGameObject.GetComponent<CurvedText>();
        //textComponent.text = sceneName;

        //Material newMaterialRef = Resources.Load<Material>("3DTextCoolVetica");
        //Font myFont = Resources.Load<Font>("coolvetica rg");

        //textComponent.font = myFont;
        //textComponent.material = newMaterialRef;
        //textComponent.text = "Hello World";

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
    private void OnDisable()
    {
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        Destroy(textMesh);
    }
}
