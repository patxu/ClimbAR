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
    private static float imgX = 2448;
    private static float imgY = 3264;
    private static int cameraSize = 5;

    // TODO: Restyle according to C# standards
    void Start () {
      int num_holds;
      int[] bb_array;
      //#if UNITY_STANDALONE_WIN
      //  // Init
      //  IntPtr bb = OpenCVFunc();
      //  num_holds = NumHolds();
      //  bb_array = new int[num_holds * 4];
      //  Marshal.Copy(bb, bb_array, 0, num_holds * 4);
      //#else
        num_holds = 2;
        bb_array = new int[]{50, 50, 10, 10, 90, 90, 10, 10};
      //#endif


        this.handHolds = new GameObject[num_holds];

        // Adjust camera zoom
        this.mainCam.orthographicSize = cameraSize/(float)2.0;

        print(num_holds); // debug

        // Instantiate handholds
        for (int i = 0; i < num_holds; i++)
        {
            float x = bb_array[i * 4]/imgX * cameraSize - cameraSize/(float)2.0;
            float y = bb_array[i * 4 + 1]/imgY * cameraSize - cameraSize/(float)2.0;
            float width = bb_array[i * 4 + 2]/imgX/(float)2.0 * cameraSize;
            float height = bb_array[i * 4 + 3]/imgY/(float)2.0 * cameraSize;

            print(x + ", " + y + "\n");

            // Create handhold object
            this.handHolds[i] = GameObject.Instantiate(Handhold);

            // TODO: Get bounds of camera and scale our position within those bounds
            // Position it in the scene, camera centered around (0, 0)
            this.handHolds[i].transform.localPosition =
                new Vector2(x + width,
                            (float)-1.0 * (y + height)
                           );
        }
        print("done");
        // TODO: Free bb_array
    }

    void Update () {

    }
}
