using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Drawing;
using System.IO;

public class TestDLL : MonoBehaviour
{
    //Constants
    static readonly int MAX_IMG_BYTES = 10000;

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

    public int segments = 10;
    LineRenderer line;

    private static int cameraSize = 5;

    // TODO: Restyle according to C# standards
    void Start () { 
        int numHolds;
        int[] boundingBoxArray;
        if (!climbSystemEnv.isWindows())
        {
            //Untested code
            //http://stackoverflow.com/questions/29171151/passing-a-byte-array-from-unity-c-sharp-into-a-c-library-method
            Image frame = Image.FromFile("pathToImage/img.png");
            byte[] imgData = imageToByteArray(frame);

            IntPtr unmanagedArray = Marshal.AllocHGlobal(MAX_IMG_BYTES);
            Marshal.Copy(imgData, 0, unmanagedArray, MAX_IMG_BYTES);
            //End untested

            IntPtr bb = OpenCVFunc();
            numHolds = NumHolds();
            boundingBoxArray = new int[numHolds * 4];
            Marshal.Copy(bb, boundingBoxArray, 0, numHolds * 4);
        }
        else
        {
            boundingBoxArray = new int[] { 50, 50, 10, 30 };
            numHolds = boundingBoxArray.Length/4;
        }

        this.handHolds = new GameObject[numHolds];

        // Adjust camera zoom
        this.mainCam.orthographicSize = cameraSize / 2f;

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
            line = this.handHolds[i].GetComponent<LineRenderer>();

            float segments = 10f;
            line.SetVertexCount((int)segments + 1);
            line.useWorldSpace = false;
            CreatePoints(segments, width, height);


            // transform handholds to be 
            this.handHolds[i].transform.localPosition =
                new Vector2(x + width,
                            (y + height) * -1f);
        }
        print("done");
    }

    void CreatePoints(float segments, float xradius, float yradius)
    {
        float x;
        float y;
        float z = 0f;

        float angle = 0f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            this.handHolds[i+1] = GameObject.Instantiate(Handhold);
            this.handHolds[i+1].transform.localPosition =
                new Vector2(x + xradius,
                (y + yradius) * -1f);

            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
            print(x + " " + y + " " + angle);
        }
    }

    void Update () {

    }

    private byte[] imageToByteArray(Image imageIn)
    {
        MemoryStream ms = new MemoryStream();
        imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        return ms.ToArray();
    }

    public Image byteArrayToImage(byte[] byteArrayIn)
    {
        MemoryStream ms = new MemoryStream(byteArrayIn);
        Image returnImage = Image.FromStream(ms);
        return returnImage;
    }
}
