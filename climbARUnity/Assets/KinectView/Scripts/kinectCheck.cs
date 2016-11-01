using UnityEngine;
using System.Collections;
using Windows.Kinect;
using UnityEngine.UI;


public class kinectCheck : MonoBehaviour {
    Text txt;
    private KinectSensor _Sensor;
    private int frameCount;
    private bool kinectConnected = false;

    void checkForKinect1()
    {
        if (kinectConnected) {
            if (!_Sensor.IsOpen)
            {
                txt.text = "Kinect Disconnected. Please Reconnect!";
                kinectConnected = false;
            }

        } else {
            _Sensor = KinectSensor.GetDefault();
            if (_Sensor != null)
            {
                txt.text = "Kinect Connected!";
                kinectConnected = true;
            }
        }
    }

    void checkForKinect2()
    {
        _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {
            txt.text = "Kinect Connected!";
        } else {
            txt.text = "Kinect Disconnected. Please Reconnect!";
        }
    }

    void checkForKinect()
    {
        _Sensor = KinectSensor.GetDefault();
        if (_Sensor.IsAvailable)
        {
            txt.text = "Kinect Connected!";
        }
        else
        {
            txt.text = "Kinect Disconnected. Please Reconnect!";
        }
    }

    // Use this for initialization
    void Start () {
        frameCount = 0;
        txt = gameObject.GetComponent<Text>();
        checkForKinect();
    }

	// Update is called once per frame
	void Update () {
        frameCount++;
        if (frameCount == 30)
        {
            checkForKinect();
            frameCount = 0;
            print("check");
        }
    }
}
