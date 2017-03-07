using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Windows.Kinect;

public class BodySourceManager : MonoBehaviour 
{
    private KinectSensor _Sensor;
    private BodyFrameReader _Reader;
    private Body[] _Data = null;
    
    public Body[] GetData()
    {
        return _Data;
    }
    

    void Start () 
    {
        _Sensor = KinectSensor.GetDefault();

        if (_Sensor != null)
        {
            _Reader = _Sensor.BodyFrameSource.OpenReader();
            
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }   
    }
    
    void Update () 
    {
        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();
            if (frame != null)
            {
                if (_Data == null)
                {
                    _Data = new Body[_Sensor.BodyFrameSource.BodyCount];
                }
                
                frame.GetAndRefreshBodyData(_Data);
                
                frame.Dispose();
                frame = null;
            }
        }
        if (Input.GetKeyDown("escape"))
        {
            GameObject bodyView = GameObject.Find("KinectBodyView");
            BodySourceView view = bodyView.GetComponent<BodySourceView>();
            view.isAHandDetected = false;

            GameObject confirmCanvas = GameObject.Find("ConfirmCanvas");
            CanvasGroup exitGroup = confirmCanvas.GetComponent<CanvasGroup>();
            exitGroup.blocksRaycasts = true;
            exitGroup.alpha = 1;
            exitGroup.interactable = true;
        }
        

        if (Input.GetKeyDown("backspace"))
        {
            if (SceneManager.GetActiveScene().name != "ClimbAR_Menu")
            {
                SceneManager.LoadSceneAsync("ClimbAR_Menu", LoadSceneMode.Single);
            }
        }

        if (Input.GetKeyDown("s"))
        {
            Camera.main.cullingMask = LayerMask.NameToLayer("Everything"); // don't show skeleton
        }

        if (Input.GetKeyDown("h"))
        {
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Skeleton")); // don't show skeleton
        }
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
