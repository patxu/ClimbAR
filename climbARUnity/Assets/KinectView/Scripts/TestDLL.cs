using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Drawing;
using System.IO;
using Windows.Kinect;

public class TestDLL : MonoBehaviour
{
    // Constants
    static readonly int MAX_IMG_BYTES = 10000;

    //Read image from Kinect
    public int ColorWidth { get; private set; }
    public int ColorHeight { get; private set; }
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

    // image variables
    // private static float imgX = 2448;
    // private static float imgY = 3264;
    private static float imgX = 100;
    private static float imgY = 100;
    private Boolean first = true;
    private Image testFrame;


    // bounding ellipse
    LineRenderer line;

    private static int cameraSize = 5;

    void Start () { 
        // TODO get this working for mac dev
        if (!climbSystemEnv.isWindows()) // currently not using this block of code
        {
            //Untested code.
            //http://stackoverflow.com/questions/29171151/passing-a-byte-array-from-unity-c-sharp-into-a-c-library-method
            Image frame = Image.FromFile("pathToImage/img.png");
            byte[] imgData = imageToByteArray(frame);

            IntPtr unmanagedArray = Marshal.AllocHGlobal(MAX_IMG_BYTES);
            Marshal.Copy(imgData, 0, unmanagedArray, MAX_IMG_BYTES);
            //End untested.

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

                var frameDesc = _Sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
                ColorWidth = frameDesc.Width;
                ColorHeight = frameDesc.Height;

                _Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGBA32, false);
                _Data = new byte[frameDesc.BytesPerPixel * frameDesc.LengthInPixels];

                if (!_Sensor.IsOpen)
                {
                    print("Sensor is not open; opening");
                    _Sensor.Open();
                }
            }
            else
            {
                print("No Kinect sensor, using static image"); // TODO integrate with Jon's logic?
            }

            this.boundingBoxArray = new int[] { 50, 50, 10, 15, 70, 70, 15, 5 };
            this.numHolds = boundingBoxArray.Length/4;
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
        this.InstantiateHandholds();
    }

    void Update () {
        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();
            
            if (frame != null)
            {
                frame.CopyConvertedFrameDataToArray(_Data, ColorImageFormat.Rgba);
                _Texture.LoadRawTextureData(_Data);
                _Texture.Apply();

                // classify image using OpenCV classifier
                this.Classify();

                // http://stackoverflow.com/questions/10894836/c-sharp-convert-image-formats-to-jpg.
                // Test code.
                if (this.first)
                {
                    //this.testFrame = (Bitmap)((new ImageConverter()).ConvertFrom(_Data));
                    //x.Save("c:\\frame.Jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    this.first = false;
                }
                // End test code.

                frame.Dispose();
                frame = null;
            }
        }
        else
        {
            Debug.Log("Using image");
        }

        this.InstantiateHandholds();
    }

    // classify image (byte array), update the number of holds, copy bounding boxes into memory
    void Classify()
    {
        int size = Marshal.SizeOf(_Data[0]) * _Data.Length;
        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(_Data, 0, ptr, _Data.Length);
        IntPtr _boundingBoxes = OpenCV.classifyImage(ptr, ColorWidth, ColorHeight);
        Marshal.FreeHGlobal(ptr);

        //this.numHolds = OpenCV.getNumHolds();
        //this.boundingBoxArray = new int[numHolds * 4];
        //Marshal.Copy(_boundingBoxes, this.boundingBoxArray, 0, this.numHolds * 4);
    }

    // update hand holds
    void InstantiateHandholds()
    {

        for (int i = 0; i < this.numHolds; i++)
        {
            int holdIndex = i * 4;
            float x = boundingBoxArray[holdIndex] / imgX * cameraSize - cameraSize / 2f;
            float y = boundingBoxArray[holdIndex + 1] / imgY * cameraSize - cameraSize / 2f;
            float width = boundingBoxArray[holdIndex + 2] / (imgX * 2.0f) * cameraSize;
            float height = boundingBoxArray[holdIndex + 3] / (imgY * 2f) * cameraSize;

            // Create handhold object and draw bounding ellipse
            line = this.handHolds[i].GetComponent<LineRenderer>();
            DrawBoundingEllipse(width, height);

            // transform handholds (camera space?)
            this.handHolds[i].transform.localPosition =
                new Vector2(x + width,
                            (y + height) * -1f);
            this.handHolds[i].GetComponent<Renderer>().enabled = true;
        }

        for (int i = this.numHolds; i < this.MAXHOLDS; i++)
        {
            this.handHolds[i].GetComponent<Renderer>().enabled = false;
        }
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
