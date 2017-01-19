using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
using Windows.Kinect;
using System.Collections;

public class OverlayDemo : MonoBehaviour
{

    // Game objects
    public GameObject[] handholds;
    public GameObject ClimbingHold; // hold prefab
    public Camera mainCam;

    public KinectClassify kinectClassifier;

    // hold bounding ellipse
    LineRenderer line;

    void Start()
    {
    }

    void Update()
    {
    }
}
