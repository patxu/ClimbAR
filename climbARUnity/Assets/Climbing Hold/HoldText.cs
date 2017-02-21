using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldText : MonoBehaviour
{

    public string sceneName;
    GameObject menuHold; // parent
    GameObject holdText; // child

    // Use this for initialization
    void Start()
    {

    }

    // must call setup script
    public void setup(string sceneName, GameObject holdText, GameObject menuHold)
    {
        this.menuHold = menuHold;
        this.holdText = holdText;
        this.holdText.transform.SetParent(this.menuHold.transform); // set empty gameobject with textmesh as child of sprite

        TextMesh textMesh = gameObject.AddComponent<TextMesh>();
        textMesh.characterSize = 0.1f;
        textMesh.fontSize = 50;
        textMesh.text = SceneUtils.SceneNameToDisplayName[sceneName];
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
