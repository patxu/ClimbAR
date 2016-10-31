using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class TestDLL : MonoBehaviour
{
    // The imported function
    #if UNITY_STANDALONE_WIN
      [DllImport("OpenCVUnity", EntryPoint = "TestSort")]
      public static extern void TestSort(int[] a, int length);
      [DllImport("OpenCVUnity", EntryPoint = "OpenCVFunc")]
      public static extern IntPtr OpenCVFunc();
      [DllImport("OpenCVUnity", EntryPoint = "NumHolds")]
      public static extern int NumHolds();
      #endif

    // Game objects
    public GameObject[] handHolds;
    public GameObject Handhold; // prefab for Handhold
    public Camera mainCam; // ...

    // Class variables
    // private static int scalingFactor = 150;
    // private static int leftShift = 5;
    // private static int downShift = 5;
    //private static float imgX = 2448;
    //private static float imgY = 3264;
    private static float imgX = 100;
    private static float imgY = 100;
    private static int cameraSize = 5;

    // TODO: Restyle according to C# standards
    void Start () { 
        int numHolds;
        int[] boundingBoxArray;
        if (!climbSystemEnv.isWindows())
        {
            IntPtr bb = OpenCVFunc();
            numHolds = NumHolds();
            boundingBoxArray = new int[numHolds * 4];
            Marshal.Copy(bb, boundingBoxArray, 0, numHolds * 4);
        }
        else
        {
            boundingBoxArray = new int[] { 50, 50, 10, 10, 90, 90, 10, 10 };
            numHolds = boundingBoxArray.Length/4;
        }


        this.handHolds = new GameObject[numHolds];

        // Adjust camera zoom
        this.mainCam.orthographicSize = cameraSize / (float)2.0;

        // Instantiate handholds
        for (int i = 0; i < numHolds; i++)
        {
            int holdIndex = i * 4;
            float x = boundingBoxArray[holdIndex] / imgX * cameraSize - cameraSize / 2f;
            float y = boundingBoxArray[holdIndex + 1] / imgY * cameraSize - cameraSize / 2f;
            float width = boundingBoxArray[holdIndex + 2] / (imgX * 2.0f) * cameraSize;
            float height = boundingBoxArray[holdIndex + 3] / (imgY * 2f) * cameraSize;

            // Create handhold object
            this.handHolds[i] = GameObject.Instantiate(Handhold);

            // transform handholds to be 
            this.handHolds[i].transform.localPosition =
                new Vector2(x + width,
                            (y + height) * -1f);
        }
        print("done");
    }

    void Update () {

    }
}
