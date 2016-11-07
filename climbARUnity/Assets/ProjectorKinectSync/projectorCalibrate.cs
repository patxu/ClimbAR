using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using Windows.Kinect;

public class projectorCalibrate : MonoBehaviour
{
    // import OpenCV dll wrapper functions
    static class OpenCV
    {
#if UNITY_STANDALONE_WIN
        [DllImport("OpenCVUnity", EntryPoint = "findProjectorBox")]
        public static extern IntPtr findProjectorBox(IntPtr redData, IntPtr greenData, IntPtr blueData, int imageWidth, int imageHeight);
#endif
    }

    private KinectSensor _Sensor;
    private ColorFrameReader _Reader;
    private byte[] _RedData;
    private byte[] _GreenData;
    private byte[] _BlueData;
    private float imageWidth;
    private float imageHeight;
    private int[] projectorCoords;
    enum Stages { RED_COLLECT, GREEN_COLLECT, BLUE_COLLECT, CALCULATING, DONE };
    private Stages currentStage;

    // Use this for initialization
    void Start()
    {
        _Sensor = KinectSensor.GetDefault();

        if (_Sensor != null)
        {
            _Reader = _Sensor.ColorFrameSource.OpenReader();

            var frameDesc = _Sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
            imageHeight = frameDesc.Height;
            imageWidth = frameDesc.Width;
            _RedData = new byte[frameDesc.BytesPerPixel * frameDesc.LengthInPixels];
            _GreenData = new byte[frameDesc.BytesPerPixel * frameDesc.LengthInPixels];
            _BlueData = new byte[frameDesc.BytesPerPixel * frameDesc.LengthInPixels];

            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }

        GetComponent<SpriteRenderer>().color = Color.red;
        currentStage = Stages.RED_COLLECT;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentStage)
        {
            case Stages.RED_COLLECT:
                if (readFrame(_RedData))
                {
                    GetComponent<SpriteRenderer>().color = Color.green;
                    currentStage = Stages.GREEN_COLLECT;
                }
                break;
            case Stages.GREEN_COLLECT:
                if (readFrame(_GreenData))
                {
                    GetComponent<SpriteRenderer>().color = Color.blue;
                    currentStage = Stages.BLUE_COLLECT;
                }
                break;
            case Stages.BLUE_COLLECT:
                if (readFrame(_BlueData))
                {
                    currentStage = Stages.CALCULATING;
                }
                break;
            case Stages.CALCULATING:
                // Copy arrays of pixel data to be sent to c++ code
                int size = Marshal.SizeOf(_RedData[0]) * _RedData.Length;
                IntPtr redArray = Marshal.AllocHGlobal(size);
                IntPtr greenArray = Marshal.AllocHGlobal(size);
                IntPtr blueArray = Marshal.AllocHGlobal(size);
                Marshal.Copy(_RedData, 0, redArray, _RedData.Length);
                Marshal.Copy(_GreenData, 0, greenArray, _GreenData.Length);
                Marshal.Copy(_BlueData, 0, blueArray, _BlueData.Length);

                // Get coordinates
                IntPtr coords = OpenCV.findProjectorBox(redArray, greenArray, blueArray, (int)imageWidth, (int)imageHeight);
                projectorCoords = new int[4];
                Marshal.Copy(coords, projectorCoords, 0, 4);
                Debug.Log(projectorCoords[0]);

                // Free data
                Marshal.FreeHGlobal(redArray);
                Marshal.FreeHGlobal(greenArray);
                Marshal.FreeHGlobal(blueArray);

                currentStage = Stages.DONE;
                break;
            case Stages.DONE:
                break;
        }
    }

    private bool readFrame(byte[] buffer)
    {
        var frame = _Reader.AcquireLatestFrame();

        if (frame != null)
        {
            frame.CopyConvertedFrameDataToArray(buffer, ColorImageFormat.Bgra);
            frame.Dispose();
            frame = null;
            return true;
        }

        Debug.Log("Can't get frame");
        return false;
    }
}
