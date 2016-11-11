using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Drawing;
using System.IO;
using Windows.Kinect;
using System.Collections;

public class TestDLL : MonoBehaviour
{
    // Constants
    static readonly int MAX_IMG_BYTES = 10000;

    // Read image from Kinect
    public float imageWidth { get; private set; }
    public float imageHeight { get; private set; }
    private KinectSensor _Sensor;
    private ColorFrameReader _Reader;
    private Texture2D _Texture;
    private byte[] _Data;

    // import OpenCV dll wrapper functions
    static class OpenCV
    {
        #if UNITY_STANDALONE_WIN
                [DllImport("OpenCVUnity", EntryPoint = "getNumHolds")]
                public static extern int getNumHolds();
                [DllImport("OpenCVUnity", EntryPoint = "classifyImage")]
                public static extern IntPtr classifyImage(IntPtr data, int width, int height);
        #endif
    }

    // Game objects
    public GameObject[] handHolds;
    public GameObject Handhold;
    public Camera mainCam;

    private readonly int MAXHOLDS = 100;
    private int numHolds = 0;
    private int[] boundingBoxArray;

    // bounding ellipse
    LineRenderer line;

    private readonly float cameraSize = 5f;

    void Start()
    {
        // TODO get this working for mac dev
        if (!climbSystemEnv.isWindows()) // currently not using this block of code
        {
            Image frame = Image.FromFile("pathToImage/img.png");
            byte[] imgData = imageToByteArray(frame);

            IntPtr unmanagedArray = Marshal.AllocHGlobal(MAX_IMG_BYTES);
            Marshal.Copy(imgData, 0, unmanagedArray, MAX_IMG_BYTES);

            //IntPtr bb = OpenCVFunc(); // defunct -- we have this.Classify() now
            //this.numHolds = NumHolds();
            //this.boundingBoxArray = new int[numHolds * 4];
            //Marshal.Copy(bb, boundingBoxArray, 0, numHolds * 4);
        }
        else
        {
            _Sensor = KinectSensor.GetDefault();

            if (_Sensor != null)
            {
                print("Acquired sensor");
                _Reader = _Sensor.ColorFrameSource.OpenReader();

                var frameDesc = _Sensor
                    .ColorFrameSource
                    .CreateFrameDescription(ColorImageFormat.Rgba);
                this.imageWidth = frameDesc.Width;
                this.imageHeight = frameDesc.Height;

                _Texture = new Texture2D(
                    (int)this.imageWidth,
                    (int)this.imageHeight,
                    TextureFormat.RGBA32,
                    false);
                _Data = new byte[frameDesc.BytesPerPixel * frameDesc.LengthInPixels];

                if (!_Sensor.IsOpen)
                {
                    print("Sensor is not open; opening");
                    _Sensor.Open();
                }
            }
            else
            {
                // TODO integrate with Jon's logic?
                print("Kinect sensor unavailable, using static image");
                this.genHardcodedBoundingBoxes();
            }

        }

        // Adjust camera zoom.
        this.mainCam.orthographicSize = cameraSize / 2f;

        // Instantiate handholds.
        // probably need to be smarter here? kinda ugly
        this.handHolds = new GameObject[this.MAXHOLDS];
        for (int i = 0; i < this.MAXHOLDS; i++)
        {
            this.handHolds[i] = GameObject.Instantiate(Handhold);
            this.handHolds[i].GetComponent<Renderer>().enabled = false;
        }

    }

    // coroutine for overlaying bounding boxes on color image
    // TODO: add skeleton overlay
    IEnumerator GrabFrameAndClassify()
    {
        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();

            if (frame != null)
            {
                print("Classifying and applying overlay");
                frame.CopyConvertedFrameDataToArray(_Data, ColorImageFormat.Bgra);
                _Texture.LoadRawTextureData(_Data);
                _Texture.Apply();

                // classify image using OpenCV classifier
                this.Classify();
                this.InstantiateHandholds();

                frame.Dispose();
                frame = null;
            }
        }
        else
        {
            Debug.Log("Using hardcoded bounding boxes or image");
        }

        yield return null;
    }

    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            print("starting coroutine");
            StartCoroutine("GrabFrameAndClassify");
        }
    }

    // classify image (byte array), update the number of holds, 
    // copy bounding boxes into memory
    void Classify()
    {
        int size = Marshal.SizeOf(_Data[0]) * _Data.Length;
        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(_Data, 0, ptr, _Data.Length);
        IntPtr _boundingBoxes = OpenCV.classifyImage(
            ptr,
            (int)imageWidth,
            (int)imageHeight);
        Marshal.FreeHGlobal(ptr);

        this.numHolds = OpenCV.getNumHolds();
        this.boundingBoxArray = new int[numHolds * 4];
        Marshal.Copy(_boundingBoxes, this.boundingBoxArray, 0, this.numHolds * 4);
    }

    // update handholds
    void InstantiateHandholds()
    {
        //TODO: get real coordinates of projector bounding box from OpenCV
        int[] testProjectorBB = new int[] { 100, 100, 900, 100, 900, 900, 100, 900};
        float[] transformedSpaceArr = transformOpenCvToUnitySpace(testProjectorBB);

        float cam_height = 2f * mainCam.orthographicSize;
        float cam_width = cam_height * mainCam.aspect;

        for (int i = 0; i < this.numHolds; i++)
        {
            int holdIndex = i * 4;
            float x = transformedSpaceArr[holdIndex] * cam_height - cam_width / 2f;
            float y = transformedSpaceArr[holdIndex + 1] * this.cameraSize - this.cameraSize / 2f;

            //float x = transformedSpaceArr[holdIndex] * this.cameraSize - this.cameraSize / 2f;
            //float y = transformedSpaceArr[holdIndex + 1] * this.cameraSize - this.cameraSize / 2f;

            float width = (transformedSpaceArr[holdIndex + 2] / 2) * cam_height; //divide by 2 because it is a radius
            float height = (transformedSpaceArr[holdIndex + 3] / 2) * cam_height;

            // float width = (transformedSpaceArr[holdIndex + 2] / 2) * this.cameraSize; //divide by 2 because it is a radius
            // float height = (transformedSpaceArr[holdIndex + 3] / 2) * this.cameraSize;

            // Create handhold object and draw bounding ellipse
            print(this.numHolds + " " + i);
            line = this.handHolds[i].GetComponent<LineRenderer>();
            DrawBoundingEllipse(width, height);

            // transform handhold (camera space?)
            this.handHolds[i].transform.localPosition =
                new Vector2(x + width,
                            (y + height) * -1f);
            this.handHolds[i].GetComponent<Renderer>().enabled = true; //not needed anymore?
        }
    }

    // simple, hardcoded bounding boxes
    void genHardcodedBoundingBoxes()
    {
        this.boundingBoxArray = new int[] { 500, 500, 100, 100, 700, 700, 150, 150 };
        this.imageWidth = 1000;
        this.imageHeight = 1000;
        this.numHolds = boundingBoxArray.Length / 4;
    }

    // draw the bounding ellipse of the climbing hold
    void DrawBoundingEllipse(float xradius, float yradius)
    {
        float x;
        float y;
        float z = 0f;

        // resolution of the sides of the ellipse
        int segments = 50;
        line.SetVertexCount(segments + 2);

        // width of line; scaled by width and height of bounding box
        float lineWidth = Math.Min(xradius, yradius) / 5f;
        line.SetWidth(lineWidth, lineWidth);

        // not currently setting the angle of ellipse
        float angle = 0f;

        for (int i = 0; i < (segments + 2); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
        }
    }

    /// <summary>
    /// Transforms coordinates given in OpenCV Space to coordinates in Unity ( 0 to 1)
    /// </summary>
    /// <param name="coordinates">int array of coordinates in the order top left (x,y), top right, bottom right, bottom left </param>
    /// <returns>float array of transformed coordinates</returns>
    private float[] transformOpenCvToUnitySpace(int[] coordinates)
    {
        int x1 = coordinates[0];
        int y1 = coordinates[1];
        int x2 = coordinates[2];
        int y2 = coordinates[3];
        int x3 = coordinates[4];
        int y3 = coordinates[5];
        int x4 = coordinates[6];
        int y4 = coordinates[7];

        float[] transformedArr = new float[this.numHolds * 4];

        float height = y4 - y1; //this is assuming y1 and y2 are approximately the same

        float leftGradient = (x4 - x1) / height;
        float rightGradient = (x3 - x2) / (y3 - y2);

        for (int i = 0; i < this.numHolds; i++)
        {
            int holdIndex = i * 4;

            // get coordinates of hold
            int currentX = this.boundingBoxArray[holdIndex];
            int currentY = this.boundingBoxArray[holdIndex + 1];
            int holdWidth = this.boundingBoxArray[holdIndex + 2];
            int holdHeight = this.boundingBoxArray[holdIndex + 3];

            //Project y on bb side left to get coordinates of the beginning of the horizonal line on which this hold belongs
            float leftX = x1 + leftGradient * (currentY - y1);

            //Project y on bb side right to get coordinates of the end of the horizonal line on which this hold belongs
            float rightX = x2 + rightGradient * (currentY - y2);

            //get length of corresponding horizontal line
            float xLength = rightX - leftX;

            //save values
            transformedArr.SetValue((currentX - leftX) / xLength, holdIndex);
            transformedArr.SetValue((currentY - y1) / height, holdIndex + 1);
            transformedArr.SetValue(holdWidth / xLength, holdIndex + 2);
            transformedArr.SetValue(holdHeight / height, holdIndex + 3);
        }

        return transformedArr;
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

    void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
    }
}
