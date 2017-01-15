using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Drawing;
using System.IO;
using Windows.Kinect;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class KinectClassify : MonoBehaviour
{
    // true if you want to use the hardcoded bounding boxes
    bool DEBUG = false;

    // Read image from Kinect
    //public float imageWidth { get; private set; }
    //public float imageHeight { get; private set; }
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

    //private int numHolds = 0;
    private int[] boundingBoxArray;

    // bounding ellipse
    LineRenderer line;

    void Start()
    {
        // start Kinect color image
        SetupKinectImage();
    }

    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            Debug.Log("starting coroutine");
            StartCoroutine("GrabFrameAndClassify");
        }
        else if (Input.GetKeyDown("t"))
        {
            foreach (var hold in handHolds)
            {
                Vector3 position = hold.transform.localPosition;
                hold.transform.localPosition = new Vector3(position.x * -1, position.y, position.z);
            }
        }
        else if (Input.GetKeyDown("q"))
        {
            Debug.Log("quitting application");
            // @ http://answers.unity3d.com/questions/899037/applicationquit-not-working-1.html
            // save any game data here
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }
    }

    /// <summary>
    /// Open Kinect Sensor, setup color image
    /// </summary>
    void SetupKinectImage()
    {
        _Sensor = KinectSensor.GetDefault();

        if (_Sensor != null)
        {
            print("Acquired sensor"); // TODO kinda buggy - doesn't actually detect in Kinect is kinected... (pat)
            _Reader = _Sensor.ColorFrameSource.OpenReader();

            // worth keeping all this as state?
            var frameDesc = _Sensor
                .ColorFrameSource
                .CreateFrameDescription(ColorImageFormat.Rgba);

            this._Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGBA32, false);
            this._Data = new byte[frameDesc.BytesPerPixel * frameDesc.LengthInPixels];

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
            //this.genHardcodedBoundingBoxes();
        }
    }


    // coroutine for overlaying bounding boxes on color image
    // TODO: add skeleton overlay
    IEnumerator GrabFrameAndClassify()
    {
        if (_Reader == null)
        {
            Debug.Log("Using hardcoded bounding boxes or image");
            yield return null;
        }

        var frame = _Reader.AcquireLatestFrame();

        if (frame != null)
        {
            int numHolds;
            int[] holdsBoundingBoxes;
            int imageWidth;
            int imageHeight;

            if (DEBUG)
            {
                Debug.Log("In debug mode");
                // simple, hardcoded bounding boxes
                //holdsBoundingBoxes = new int[] { 500, 500, 100, 100, 700, 700, 150, 150 };
                holdsBoundingBoxes = new int[] { 0, 0, 100, 100, 1800, 900, 100, 100 };
                numHolds = holdsBoundingBoxes.Length / 4;

                imageWidth = 1000;
                imageHeight = 1000;
            }
            else
            {
                print("Getting Kinect frame and classifying");
                frame.CopyConvertedFrameDataToArray(this._Data, ColorImageFormat.Bgra);
                this._Texture.LoadRawTextureData(this._Data);
                this._Texture.Apply();

                // classify image using OpenCV classifier
                numHolds = OpenCV.getNumHolds();

                FrameDescription frameDesc = _Sensor
                    .ColorFrameSource
                    .CreateFrameDescription(ColorImageFormat.Bgra);
                imageWidth = frameDesc.Width;
                imageHeight = frameDesc.Height;

                holdsBoundingBoxes = ClassifyImage(numHolds, imageWidth, imageHeight);
            }

            //TODO: get real coordinates of projector bounding box from OpenCV; move to DEBUG block
            //int[] projectorBounds = new int[] { 0, 0, 1920, 0, 1920, 1080, 0, 1080 };
            Vector2 topLeft = StateManager.instance.kinectUpperLeft; // ClimbARUtils.worldSpaceToFraction(StateManager.instance.kinectUpperLeft.x, StateManager.instance.kinectUpperLeft.x, mainCam);
            Debug.Log(topLeft);
            topLeft.Scale(ClimbARUtils.kinectScale);
            Vector2 topRight = StateManager.instance.kinectUpperRight; // ClimbARUtils.worldSpaceToFraction(StateManager.instance.kinectUpperRight.x, StateManager.instance.kinectUpperRight.x, mainCam);
            Debug.Log(topRight);
            topRight.Scale(ClimbARUtils.kinectScale);
            Vector2 bottomRight = StateManager.instance.kinectLowerRight; //ClimbARUtils.worldSpaceToFraction(StateManager.instance.kinectLowerRight.x, StateManager.instance.kinectLowerRight.x, mainCam);
            Debug.Log(bottomRight);
            bottomRight.Scale(ClimbARUtils.kinectScale);
            Vector2 bottomLeft = StateManager.instance.kinectLowerLeft; //ClimbARUtils.worldSpaceToFraction(StateManager.instance.kinectLowerLeft.x, StateManager.instance.kinectLowerLeft.x, mainCam);
            Debug.Log(bottomLeft);
            bottomLeft.Scale(ClimbARUtils.kinectScale);

            int[] projectorBounds = new int[] { (int)topLeft.x, (int)topLeft.y, (int)topRight.x, (int)topRight.y, (int)bottomRight.x, (int)bottomRight.y, (int)bottomLeft.x, (int)bottomLeft.y };

            float[] holdsProjectorTransformed;

            if (!StateManager.instance.debugView)
            {
                holdsProjectorTransformed = transformOpenCvToUnitySpace(projectorBounds, holdsBoundingBoxes);
            }
            else
            {
                holdsProjectorTransformed = new float[holdsBoundingBoxes.Length];
                for (int i = 0; i < holdsBoundingBoxes.Length; i++)
                {
                    holdsProjectorTransformed[i] = (float)holdsBoundingBoxes[i];
                }

            }
            InstantiateHandholds(numHolds, this.mainCam, holdsProjectorTransformed);
            if (!DEBUG)
            {
                frame.Dispose();
                frame = null;
            }
        }

        yield return null;
    }

    // classify image (byte array), update the number of holds, 
    // copy bounding boxes into memory
    int[] ClassifyImage(int numHolds, int imageWidth, int imageHeight)
    {
        int size = Marshal.SizeOf(this._Data[0]) * this._Data.Length;
        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(this._Data, 0, ptr, this._Data.Length);
        IntPtr _boundingBoxes = OpenCV.classifyImage(
            ptr,
            imageWidth,
            imageHeight);
        Marshal.FreeHGlobal(ptr);

        int[] holdBoundingBoxes = new int[numHolds * 4];
        Marshal.Copy(_boundingBoxes, holdBoundingBoxes, 0, numHolds * 4);
        return holdBoundingBoxes;
    }



    // update handholds
    void InstantiateHandholds(int numHolds, Camera cam, float[] projectorTransformation)
    {
        print("Instantiating " + numHolds + " handholds");
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;


        if (this.handHolds.Length != 0)
        {
            for (int i = 0; i < this.handHolds.Length; i++)
            {
                // Make sure this hold has not been manually deleted by the user due to a false positive in the classification stage
                if (this.handHolds[i])
                {
                    CircleCollider2D col = this.handHolds[i].GetComponent<CircleCollider2D>();
                    if (col)
                    {
                        DestroyImmediate(col);
                    }
                    Destroy(this.handHolds[i]);
                }
            }
        }
        this.handHolds = new GameObject[numHolds];

        for (int i = 0; i < numHolds; i++)
        {
            int holdIndex = i * 4;
            float x = projectorTransformation[holdIndex] * camWidth - camWidth / 2f;
            float y = projectorTransformation[holdIndex + 1] * camHeight - camHeight / 2f;



            float width = (projectorTransformation[holdIndex + 2] / 2) * camWidth; //divide by 2 because it is a radius
            float height = (projectorTransformation[holdIndex + 3] / 2) * camHeight;

            this.handHolds[i] = GameObject.Instantiate(Handhold);
            this.handHolds[i].name = "Handhold " + i;

            Vector2 pos = new Vector2(x + width,
                            (y + height) * -1f);

            if (!StateManager.instance.debugView)
            {
                pos.x = pos.x * -1;
            }
            this.handHolds[i].transform.localPosition = pos;

            Rigidbody2D rigid = this.handHolds[i].AddComponent<Rigidbody2D>();
            rigid.isKinematic = true;

            CircleCollider2D col = this.handHolds[i].AddComponent<CircleCollider2D>();
            col.radius = 0.2f; //(float)Math.Max(width, height);
            col.enabled = true;
            col.isTrigger = true;
            // Create handhold object and draw bounding ellipse
            line = this.handHolds[i].GetComponent<LineRenderer>();
            DrawBoundingEllipse(width, height);
        }
    }

    Vector2 getPositionForGameObject(Vector2 position, bool debug)
    {
        return position;
    }

    // draw the bounding ellipse of the climbing hold
    void DrawBoundingEllipse(float xradius, float yradius)

    {
        line.SetColors(UnityEngine.Color.red, UnityEngine.Color.red);
        //line.material = new Material(Shader.Find("Particles/Additive"));

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

            line.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }
    }

    /// <summary>
    /// Transforms coordinates given in OpenCV Space to coordinates in Unity ( 0 to 1)
    /// </summary>
    /// <param name="coordinates">int array of coordinates in the order top left (x,y), top right, bottom right, bottom left </param>
    /// <returns>float array of transformed coordinates</returns>
    private float[] transformOpenCvToUnitySpace(int[] coordinates, int[] boundingBoxArray)
    {
        int x1 = coordinates[0];
        int y1 = coordinates[1];
        int x2 = coordinates[2];
        int y2 = coordinates[3];
        int x3 = coordinates[4];
        int y3 = coordinates[5];
        int x4 = coordinates[6];
        int y4 = coordinates[7];

        float[] transformedArr = new float[boundingBoxArray.Length];

        float height = y4 - y1; //this is assuming y1 and y2 are approximately the same

        float leftGradient = (x4 - x1) / height;
        float rightGradient = (x3 - x2) / (y3 - y2);

        for (int i = 0; i < boundingBoxArray.Length / 4; i++)
        {
            //Debug.Log("Hold: ");
            int holdIndex = i * 4;

            // get coordinates of hold
            int currentX = boundingBoxArray[holdIndex];
            int currentY = boundingBoxArray[holdIndex + 1];
            int holdWidth = boundingBoxArray[holdIndex + 2];
            int holdHeight = boundingBoxArray[holdIndex + 3];

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

    //----------------------------------------------------
    // TODO: Test these functions!!!
    //----------------------------------------------------

    float getDistanceBetweenHolds(GameObject hold1, GameObject hold2)
    {
        float x1, x2, y1, y2;
        x1 = hold1.transform.position.x;
        y1 = hold1.transform.position.y;
        x2 = hold2.transform.position.x;
        y2 = hold2.transform.position.y;

        return Mathf.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
    }

    GameObject getNearestHold(GameObject[] handHolds, GameObject currentHold)
    {
        // input tests i
        if (handHolds == null)
        {
            Console.Error.WriteLine("Handholds array is null!");
            return null;
        }
        if (handHolds.Length == 0)
        {
            Console.Error.WriteLine("Handholds array is empty");
            return null;
        }


        float minDistance = float.MaxValue;
        GameObject minHold = null;
        for (int i = 0; i < handHolds.Length; i++)
        {
            if (handHolds[i] != currentHold && getDistanceBetweenHolds(currentHold, handHolds[i]) < minDistance)
            {
                minDistance = getDistanceBetweenHolds(currentHold, handHolds[i]);
                minHold = handHolds[i];
            }
        }

        return minHold;
    }


    GameObject getNearestHoldAbove(GameObject[] handHolds, GameObject currentHold)
    {
        float minDistance = float.MaxValue;
        GameObject minHold = null;
        for (int i = 0; i < handHolds.Length; i++)
        {
            if (handHolds[i] != currentHold &&
                handHolds[i].transform.position.y > currentHold.transform.position.y &&
                getDistanceBetweenHolds(currentHold, handHolds[i]) < minDistance)
            {
                minDistance = getDistanceBetweenHolds(currentHold, handHolds[i]);
                minHold = handHolds[i];
            }
        }

        return minHold;
    }

    GameObject getNearestHoldBelow(GameObject[] handHolds, GameObject currentHold)
    {
        float minDistance = float.MaxValue;
        GameObject minHold = null;
        for (int i = 0; i < handHolds.Length; i++)
        {
            if (handHolds[i] != currentHold &&
                handHolds[i].transform.position.y < currentHold.transform.position.y &&
                getDistanceBetweenHolds(currentHold, handHolds[i]) < minDistance)
            {
                minDistance = getDistanceBetweenHolds(currentHold, handHolds[i]);
                minHold = handHolds[i];
            }
        }

        return minHold;
    }

    GameObject getNearestHoldRight(GameObject[] handHolds, GameObject currentHold)
    {
        float minDistance = float.MaxValue;
        GameObject minHold = null;
        for (int i = 0; i < handHolds.Length; i++)
        {
            if (handHolds[i] != currentHold &&
                handHolds[i].transform.position.x > currentHold.transform.position.x &&
                getDistanceBetweenHolds(currentHold, handHolds[i]) < minDistance)
            {
                minDistance = getDistanceBetweenHolds(currentHold, handHolds[i]);
                minHold = handHolds[i];
            }
        }

        return minHold;
    }

    GameObject getNearestHoldLeft(GameObject[] handHolds, GameObject currentHold)
    {
        float minDistance = float.MaxValue;
        GameObject minHold = null;
        for (int i = 0; i < handHolds.Length; i++)
        {
            if (handHolds[i] != currentHold &&
                handHolds[i].transform.position.x < currentHold.transform.position.x &&
                getDistanceBetweenHolds(currentHold, handHolds[i]) < minDistance)
            {
                minDistance = getDistanceBetweenHolds(currentHold, handHolds[i]);
                minHold = handHolds[i];
            }
        }

        return minHold;
    }

}


