using UnityEngine;
using System.Runtime.InteropServices;
using System;
using Windows.Kinect;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class KinectClassify : MonoBehaviour
{
    // true if you want to use the hardcoded bounding boxes
    private bool DEBUG = false;

    public readonly string ClassifyImage = "GrabFrameAndClassify";
    public readonly string ClassifyImageWithDelay = "GrabFrameAndClassifyWithDelay";

    // import OpenCV dll wrapper functions
    static class OpenCV
    {
        [DllImport("OpenCVUnity", EntryPoint = "classifyImage")]
        public static extern IntPtr classifyImage(string classifierPath, IntPtr data, int width, int height);

        [DllImport("OpenCVUnity", EntryPoint = "cleanupBBArray")]
        public static extern void cleanupBBArray();
    }

    // to access the Kinect
    private KinectSensor _Sensor;
    private ColorFrameReader _Reader;
    private Texture2D _Texture;
    private byte[] _Data;

    // Game objects
    public GameObject[] handholds = new GameObject[0];
    public GameObject Handhold;
    public Camera mainCam;
    //public string classifierPath = "C:\\cs98-senior-project\\OpenCV_files\\cascade_demo.xml";
    //public string classifierPath = "C:\\cs98-senior-project\\OpenCV_files\\cascade_17_newgym_lbp.xml";
    public string classifierPath = "C:\\cs98-senior-project\\OpenCV_files\\cascade.xml";
    bool classifyRunning = false;

    void Start()
    {
        _Sensor = KinectSensor.GetDefault();

        if (_Sensor != null)
        {
            _Reader = _Sensor.ColorFrameSource.OpenReader();

            var frameDesc = _Sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);

            _Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGBA32, false);
            _Data = new byte[frameDesc.BytesPerPixel * frameDesc.LengthInPixels];

            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }
        else
        {
            ClimbARUtils.LogError("cannot get Kinect sensor");
        }
        GameObject bodyView = GameObject.Find("KinectBodyView");
        BodySourceView view = bodyView.GetComponent<BodySourceView>();
        view.isAHandDetected = false;
    }

    void Update()
    {
        if (Input.GetKeyDown("c"))
        {

            if (!classifyRunning)
            {
                StartCoroutine("GrabFrameAndClassify");
            }
            else
            {
                Debug.Log("Routine already running");
            }
        }
        else if (Input.GetKeyDown("t"))
        {
            foreach (GameObject hold in handholds)
            {
                Vector3 position = hold.transform.localPosition;
                hold.transform.localPosition =
                    new Vector3(position.x * -1, position.y, position.z);
            }
        }


        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();

            if (frame != null)
            {
                frame.CopyConvertedFrameDataToArray(_Data, ColorImageFormat.Rgba);
                _Texture.LoadRawTextureData(_Data);
                //_Texture.Apply();

                frame.Dispose();
                frame = null;
            }
        }
    }

    // coroutine for overlaying bounding boxes on color image
    IEnumerator GrabFrameAndClassify()
    {
        classifyRunning = true;
        Debug.Log("starting classification coroutine");


        if (_Reader == null)
        {
            Debug.Log("Using hardcoded bounding boxes or image");
            yield return null;
        }
        ColorFrame frame = _Reader.AcquireLatestFrame();
        if (frame != null)
        {
            int numHolds;
            float[] holdsBoundingBoxes;
            int imageWidth;
            int imageHeight;

            if (DEBUG)
            {
                Debug.Log("In debug mode; using hardcoded bounding boxes");
                //holdsBoundingBoxes = new int[] { 500, 500, 100, 100, 700, 700, 150, 150 };
                holdsBoundingBoxes = new float[] { 500, 500, 100, 100 };
                numHolds = holdsBoundingBoxes.Length / 4;

                imageWidth = 1000;
                imageHeight = 1000;
            }
            else
            {
                // don't apply texture, just load it for classification
                frame.CopyConvertedFrameDataToArray(
                    _Data,
                    ColorImageFormat.Bgra);
                _Texture.LoadRawTextureData(_Data);

                // classify image using OpenCV classifier

                FrameDescription frameDesc = _Sensor
                    .ColorFrameSource
                    .CreateFrameDescription(ColorImageFormat.Bgra);
                imageWidth = frameDesc.Width;
                imageHeight = frameDesc.Height;
                holdsBoundingBoxes = classifyWithOpenCV(imageWidth, imageHeight);
                if (holdsBoundingBoxes[0] < 0)
                {
                    if (!DEBUG)
                    {
                        frame.Dispose();
                        frame = null;
                    }
                    ClimbARUtils.LogError("Error with classifying. Exiting coroutine");
                    classifyRunning = false;
                    yield break;
                }
                numHolds = holdsBoundingBoxes.Length / 4;
            }

            float[] projectorBounds = StateManager.instance.getProjectorBounds();
            float[] holdsProjectorTransformed;

            if (!StateManager.instance.debugView)
            {
                holdsProjectorTransformed =
                    ClimbARTransformation.transformOpenCvToUnitySpace(
                        projectorBounds,
                        holdsBoundingBoxes);
            }
            else
            {
                holdsProjectorTransformed = new float[holdsBoundingBoxes.Length];
                for (int i = 0; i < holdsBoundingBoxes.Length; i++)
                {
                    holdsProjectorTransformed[i] = (float)holdsBoundingBoxes[i];
                }

            }

            Debug.Log("instantiating " + numHolds + " holds");

            cleanHandHolds(ref this.handholds);
            this.handholds = ClimbARHandhold.InstantiateHandholds(
                this.Handhold,
                this.mainCam,
                holdsProjectorTransformed);

            // persist holds
            for (int i = 0; i < this.handholds.Length; i++)
            {
                DontDestroyOnLoad(this.handholds[i]);
            }

            if (!DEBUG)
            {
                frame.Dispose();
                frame = null;
            }
        }
        else
        {
            ClimbARUtils.LogError("Frame was null");
        }
        classifyRunning = false;
    }

    IEnumerable GrabFrameAndClassifyWithDelay(int delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine("GrabFrameAndClassify");
    }

    void cleanHandHolds(ref GameObject[] handholds)
    {
        for (int i = 0; i < handholds.Length; i++)
        {
            // Make sure this hold has not been manually deleted by the user
            // due to a false positive in the classification stage
            if (handholds[i])
            {
                CircleCollider2D col = handholds[i].GetComponent<CircleCollider2D>();
                if (col)
                {
                    DestroyImmediate(col);
                }
                Destroy(handholds[i]);
            }
        }
    }

    // classify image (byte array), update the number of holds,
    // copy bounding boxes into memory
    float[] classifyWithOpenCV(int imageWidth, int imageHeight)
    {
        int size = Marshal.SizeOf(_Data[0]) * _Data.Length;
        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(_Data, 0, ptr, _Data.Length);
        IntPtr _boundingBoxes = OpenCV.classifyImage(
            classifierPath,
            ptr,
            imageWidth,
            imageHeight);
        Marshal.FreeHGlobal(ptr);

        int[] holdCount = new int[1];
        Marshal.Copy(_boundingBoxes, holdCount, 0, 1);

        if (holdCount[0] < 1)
        {
            ClimbARUtils.LogError("Error with classifier");
            OpenCV.cleanupBBArray();
            float[] error = new float[1];
            error[0] = -1f;
            return error;
        }

        int[] holdBoundingBoxesTmp = new int[(holdCount[0] * 4) + 1];
        Marshal.Copy(_boundingBoxes, holdBoundingBoxesTmp, 0, (holdCount[0] * 4) + 1);
        OpenCV.cleanupBBArray();
        int[] holdBoundingBoxes = new int[holdCount[0] * 4];

        Array.Copy(holdBoundingBoxesTmp, 1, holdBoundingBoxes, 0, holdCount[0] * 4);

        return Array.ConvertAll(holdBoundingBoxes, x => (float)x);
    }

}
