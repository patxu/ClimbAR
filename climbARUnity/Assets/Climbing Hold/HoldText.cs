using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldText : MonoBehaviour
{

    public string sceneName;

    // Use this for initialization
    void Start()
    {

    }

    // must call setup script
    public void setup(string sceneName, GameObject holdText, GameObject menuHold)
    {
        this.addText(SceneUtils.SceneNameToDisplayName[sceneName], holdText, menuHold); 
    }

    public void addText(string text, GameObject holdText, GameObject menuHold)
    {
        holdText.transform.SetParent(menuHold.transform); // set empty gameobject with textmesh as child of sprite

        TextMesh textMesh = holdText.AddComponent<TextMesh>();
        textMesh.characterSize = 0.12f;
        textMesh.fontSize = 40;
        textMesh.text = text;
        textMesh.transform.position = menuHold.transform.position;
        textMesh.anchor = TextAnchor.MiddleLeft;

        var parentRenderer = menuHold.GetComponent<SpriteRenderer>();
        var renderer = holdText.GetComponent<MeshRenderer>();
        renderer.sortingLayerID = parentRenderer.sortingLayerID;
        renderer.sortingOrder = parentRenderer.sortingOrder + 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
