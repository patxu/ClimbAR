using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class projectorCalibrate : MonoBehaviour
{

    //Red frame
    //Green frame
    // Blue frame

    private KinectSensor _Sensor;
    private ColorFrameReader _Reader;
    private byte[] _RedData;
    private byte[] _GreenData;
    private byte[] _BlueData;
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
