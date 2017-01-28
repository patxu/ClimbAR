using UnityEngine;
using System.Runtime.InteropServices;
using System;
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

    // import OpenCV dll wrapper functions
    static class OpenCV
    {
        [DllImport("OpenCVUnity", EntryPoint = "getNumHolds")]
        public static extern int getNumHolds();
        [DllImport("OpenCVUnity", EntryPoint = "classifyImage")]
        public static extern IntPtr classifyImage(string classifierPath, IntPtr data, int width, int height);
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
    public string classifierPath = "C:\\cs98-senior-project\\OpenCV_files\\cascade_demo.xml";

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
            Debug.Log("cannot get Kinect sensor");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            print("starting classification coroutine");
            StartCoroutine("GrabFrameAndClassify");
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
                holdsBoundingBoxes = new float[] { 0, 0, 100, 100, 1800, 900, 100, 100 };
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
                //numHolds = OpenCV.getNumHolds();

                FrameDescription frameDesc = _Sensor
                    .ColorFrameSource
                    .CreateFrameDescription(ColorImageFormat.Bgra);
                imageWidth = frameDesc.Width;
                imageHeight = frameDesc.Height;

                holdsBoundingBoxes = classifyWithOpenCV(10, imageWidth, imageHeight);
                numHolds = OpenCV.getNumHolds();
                Debug.Log("GetNumHolds found " + numHolds.ToString());
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

            print("instantiating " + numHolds + " holds");

            cleanHandHolds(ref this.handholds);
            this.handholds = ClimbARHandhold.InstantiateHandholds(
                this.Handhold,
                numHolds,
                this.mainCam,
                holdsProjectorTransformed);

            if (!DEBUG)
            {
                frame.Dispose();
                frame = null;
            }
        }

        yield return null;
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
    float[] classifyWithOpenCV(int numHolds, int imageWidth, int imageHeight)
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
        Debug.Log("Found: " + holdCount[0].ToString());

        if (holdCount[0] < 1)
        {
            Debug.Log("Error with classifier!!!");
            return null;
        }

        int[] holdBoundingBoxesTmp = new int[(holdCount[0] * 4) + 1];
        Marshal.Copy(_boundingBoxes, holdBoundingBoxesTmp, 0, (holdCount[0] * 4) + 1);

        int[] holdBoundingBoxes = new int[holdCount[0] * 4];

        Array.Copy(holdBoundingBoxesTmp, 1, holdBoundingBoxes, 0, holdCount[0] * 4);

        return Array.ConvertAll(holdBoundingBoxes, x => (float)x);
    }

}
