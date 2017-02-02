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
            StartCoroutine(TransitionToSceneWithDelay(SceneUtils.Names.manualSync, 0.3f));
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
        if (Input.GetKeyDown("escape"))
        {
            if (Application.isEditor)
            {
                Debug.Log("Cannot quit the application (Application is editor).");
            }
            else
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
    }

    IEnumerator TransitionToSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
}
