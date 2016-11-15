using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Windows.Kinect;
using UnityEngine.UI;


public class kinectCheck : MonoBehaviour
{
    private Text txt;
    private KinectSensor _Sensor;

    private void onIsAvailableChanged(object sensor, System.EventArgs args)
    {
        checkKinectConnection((KinectSensor)sensor);
    }

    void checkKinectConnection(KinectSensor sensor)
    {
        if (sensor.IsAvailable)
        {
            txt.text = "Connected! Transitioning...";
            StartCoroutine(TransitionToSceneWithDelay("2a_ManualSync", 2f));
        }
        else
        {
            txt.text = "Kinect Disconnected. Please Reconnect!";
        }
    }

    // Use this for initialization
    void Start()
    {
        txt = gameObject.GetComponent<Text>();
        _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {
            _Sensor.Open();
        }
        checkKinectConnection(_Sensor);
        _Sensor.IsAvailableChanged += onIsAvailableChanged;
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator TransitionToSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
}
