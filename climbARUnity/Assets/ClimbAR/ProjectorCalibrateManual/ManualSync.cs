using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

public class ManualSync : MonoBehaviour
{
    //private constants
    private float BORDER_WIDTH = 0.05f
        ;
    private string path = "Assets/ClimbAR/ProjectorCalibrateManual/SavedState/SavedState.xml";
    // Game objects
    public GameObject[] cornerCircles;
    public float[] cornerCircleCoordinates;
    public GameObject CornerCircle;
    public Camera mainCam;

    // Use this for initialization
    void Start()
    {
        //if saved state file exists
        //load holds and transition to next scene

        if (File.Exists(path)) {
            print("The file exists.");

            XmlSerializer xmlSearializer = new XmlSerializer(typeof(float[]));

            using (FileStream fs = File.Open(path, FileMode.Open))
            {

                cornerCircleCoordinates =  (float[]) xmlSearializer.Deserialize(fs);
               foreach(float f in cornerCircleCoordinates)
                {
                    print(f);
                } 
            }

        } else
        {
            print("File not found");
            string path = Directory.GetCurrentDirectory();
            print(path);
        }

        //else
        InitCornerCircles();
        DrawBorder(BORDER_WIDTH);
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
        // continue to next scene, saving the coordinates of the corner circles as state
        if (Input.GetKeyDown("space"))
        {

            //debug
            cornerCircleCoordinates = new float[] { 1.4363f, 2.34563f, 3.3456f };
            //end debug

            float upperY = (this.cornerCircles[0].transform.localPosition.y + this.cornerCircles[1].transform.localPosition.y) / 2;
            float lowerY = (this.cornerCircles[2].transform.localPosition.y + this.cornerCircles[3].transform.localPosition.y) / 2;

            // 0,0 is top left, +y points down
            StateManager.instance.kinectUpperLeft = ClimbARUtils.worldSpaceToFraction(
                this.cornerCircles[0].transform.localPosition.x,
                upperY,
                mainCam);
            StateManager.instance.kinectUpperRight = ClimbARUtils.worldSpaceToFraction(
                this.cornerCircles[1].transform.localPosition.x,
                upperY,
                mainCam);
            StateManager.instance.kinectLowerRight = ClimbARUtils.worldSpaceToFraction(
                this.cornerCircles[2].transform.localPosition.x,
                lowerY,
                mainCam);
            StateManager.instance.kinectLowerLeft = ClimbARUtils.worldSpaceToFraction(
                this.cornerCircles[3].transform.localPosition.x,
                lowerY,
                mainCam);

            //serialize here
            XmlSerializer xmlSearializer = new XmlSerializer(typeof(float[]));
            FileStream file = File.Create(path);

            xmlSearializer.Serialize(file, cornerCircleCoordinates);
            file.Close();

            SceneManager.LoadScene(SceneUtils.SceneNames.holdSetup);
            //SceneManager.LoadScene(SceneUtils.Names.demo);
        }

        // reset to outer corners
        if (Input.GetKeyDown("r"))
        {
            if (this.cornerCircles != null)
            {
                this.cornerCircles[0].transform.localPosition =
                    ClimbARUtils.fractionToWorldSpace(0f, 0f, this.mainCam);
                this.cornerCircles[1].transform.localPosition =
                    ClimbARUtils.fractionToWorldSpace(1f, 0f, this.mainCam);
                this.cornerCircles[2].transform.localPosition =
                    ClimbARUtils.fractionToWorldSpace(1f, 1f, this.mainCam);
                this.cornerCircles[3].transform.localPosition =
                    ClimbARUtils.fractionToWorldSpace(0f, 1f, this.mainCam);
            }
        }

        if (Input.GetKeyDown("escape"))
        {
            GameObject confirmCanvas = GameObject.Find("ConfirmCanvas");
            CanvasGroup exitGroup = confirmCanvas.GetComponent<CanvasGroup>();
            exitGroup.blocksRaycasts = true;
            exitGroup.alpha = 1;
            exitGroup.interactable = true;
        }
    }

    /// <summary>
    /// 0, 1, 2, 3 -> top left, top right, bottom right, bottom left
    /// </summary>
    void InitCornerCircles()
    {
        this.cornerCircles = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            this.cornerCircles[i] = GameObject.Instantiate(CornerCircle);
            this.cornerCircles[i].name = "CornerCircle " + (i);
        }
        this.cornerCircles[0].transform.localPosition =
            ClimbARUtils.fractionToWorldSpace(0.1f, 0.1f, this.mainCam);
        this.cornerCircles[1].transform.localPosition =
            ClimbARUtils.fractionToWorldSpace(0.9f, 0.1f, this.mainCam);
        this.cornerCircles[2].transform.localPosition =
            ClimbARUtils.fractionToWorldSpace(.9f, .9f, this.mainCam);
        this.cornerCircles[3].transform.localPosition =
            ClimbARUtils.fractionToWorldSpace(0.1f, 0.9f, this.mainCam);
    }

    void DrawBorder(float width)
    {
        Vector2 topLeft = ClimbARUtils.fractionToWorldSpace(0f, 0f, this.mainCam); ;
        Vector2 bottomLeft = ClimbARUtils.fractionToWorldSpace(0f, 1f, this.mainCam);
        Vector2 topRight = ClimbARUtils.fractionToWorldSpace(1f, 0f, this.mainCam);
        Vector2 bottomRight = ClimbARUtils.fractionToWorldSpace(1f, 1f, this.mainCam);

        DrawLine(topLeft, bottomLeft, width);
        DrawLine(topRight, bottomRight, width);
        DrawLine(topLeft, topRight, width);
        DrawLine(bottomLeft, bottomRight, width);
    }

    private void DrawLine(Vector2 start, Vector2 end, float width)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.startWidth = width;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
