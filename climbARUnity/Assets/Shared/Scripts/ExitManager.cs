using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitManager : MonoBehaviour {
    public Button confirmButton;
    public Button cancelButton;
	// Use this for initialization
	void Start () {
        confirmButton.onClick.AddListener(ExitConfirmed);
        cancelButton.onClick.AddListener(ExitCancled);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ExitConfirmed()
    {
        if (Application.isEditor)
        {
            Debug.Log("Cannot quit the application (Application is editor).");
            ExitCancled();
        }
        else
        { 
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }

    public void ExitCancled()
    { 
        GameObject bodyView = GameObject.Find("KinectBodyView");
        BodySourceView view = bodyView.GetComponent<BodySourceView>();
        view.shouldShowTextMesh = true;

        GameObject confirmCanvas = GameObject.Find("ConfirmCanvas");
        CanvasGroup exitGroup = confirmCanvas.GetComponent<CanvasGroup>();
        exitGroup.blocksRaycasts = false;
        exitGroup.alpha = 0;
        exitGroup.interactable = false;
    }
}
