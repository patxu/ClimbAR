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
        holdText.transform.SetParent(menuHold.transform); // set empty gameobject with textmesh as child of sprite

        TextMesh textMesh = holdText.AddComponent<TextMesh>();
        textMesh.characterSize = 0.1f;
        textMesh.fontSize = 25;
        textMesh.text = SceneUtils.SceneNameToDisplayName[sceneName];
        textMesh.transform.localPosition = menuHold.transform.localPosition;
        //textMesh.anchor = TextAnchor.MiddleCenter;

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
