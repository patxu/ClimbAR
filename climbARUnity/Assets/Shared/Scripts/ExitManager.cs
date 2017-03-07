using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitManager : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

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
        if (bodyView != null)
        {
            BodySourceView view = bodyView.GetComponent<BodySourceView>();
            view.isAHandDetected = true;
        }

        GameObject confirmCanvas = GameObject.Find("ConfirmCanvas");
        CanvasGroup exitGroup = confirmCanvas.GetComponent<CanvasGroup>();
        exitGroup.blocksRaycasts = false;
        exitGroup.alpha = 0;
        exitGroup.interactable = false;
    }
}
