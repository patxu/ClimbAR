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
    private string path = "Assets/ClimbAR/ProjectorCalibrateManual/SavedState.xml";
    // Game objects
    public GameObject[] cornerCircles;
    public float[] cornerCircleCoordinates; //used for serialization - gets values from cornerCircles
    public GameObject CornerCircle;
    public Camera mainCam;

    // Use this for initialization
    void Start()
    {

        InitCornerCircles();

        //if saved state file exists
        //reposition circles
        if (File.Exists(path))
        {
            XmlSerializer xmlSearializer = new XmlSerializer(typeof(float[]));

            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                cornerCircleCoordinates = (float[])xmlSearializer.Deserialize(fs);
                PositionCircles(cornerCircleCoordinates);
            }
        }

        DrawBorder(BORDER_WIDTH);
    }

    // Update is called once per frame
    void Update()
    {
        // continue to next scene, saving the coordinates of the corner circles as state
        if (Input.GetKeyDown("space"))
        {
            cornerCircleCoordinates = GetCoordinateFloatArray();
            RecordBounds(cornerCircleCoordinates);
            Serialize(path, cornerCircleCoordinates);

            SceneManager.LoadScene(SceneUtils.SceneNames.holdSetup);
        }

        // reset to outer corners
        if (Input.GetKeyDown("r"))
        {
            Vector2 topLeft = ClimbARUtils.fractionToWorldSpace(0f, 0f, this.mainCam);
            Vector2 topRight = ClimbARUtils.fractionToWorldSpace(1f, 0f, this.mainCam);
            Vector2 bottomRight = ClimbARUtils.fractionToWorldSpace(1f, 1f, this.mainCam);
            Vector2 bottomLeft = ClimbARUtils.fractionToWorldSpace(0f, 1f, this.mainCam);

            PositionCircles(topLeft, topRight, bottomRight, bottomLeft);
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

    //Generates array used for serialization
    private float[] GetCoordinateFloatArray()
    {
        float[] arr = new float[8];

        arr[0] = this.cornerCircles[0].transform.localPosition.x;
        arr[1] = this.cornerCircles[0].transform.localPosition.y;
        arr[2] = this.cornerCircles[1].transform.localPosition.x;
        arr[3] = this.cornerCircles[1].transform.localPosition.y;
        arr[4] = this.cornerCircles[2].transform.localPosition.x;
        arr[5] = this.cornerCircles[2].transform.localPosition.y;
        arr[6] = this.cornerCircles[3].transform.localPosition.x;
        arr[7] = this.cornerCircles[3].transform.localPosition.y;

        return arr;
    }

    private void RecordBounds(float[] cornerCircleCoordinates)
    {
        float upperY = (cornerCircleCoordinates[1] + cornerCircleCoordinates[3]) / 2;
        float lowerY = (cornerCircleCoordinates[5] + cornerCircleCoordinates[7]) / 2;

        // 0,0 is top left, +y points down
        StateManager.instance.kinectUpperLeft = ClimbARUtils.worldSpaceToFraction(
            cornerCircleCoordinates[0],
            cornerCircleCoordinates[1],
            mainCam);
        StateManager.instance.kinectUpperRight = ClimbARUtils.worldSpaceToFraction(
            cornerCircleCoordinates[2],
            cornerCircleCoordinates[3],
            mainCam);
        StateManager.instance.kinectLowerRight = ClimbARUtils.worldSpaceToFraction(
            cornerCircleCoordinates[4],
            cornerCircleCoordinates[5],
            mainCam);
        StateManager.instance.kinectLowerLeft = ClimbARUtils.worldSpaceToFraction(
            cornerCircleCoordinates[6],
            cornerCircleCoordinates[7],
            mainCam);
    }

    private void Serialize(string path, float[] cornerCircleCoordinates)
    {
        XmlSerializer xmlSearializer = new XmlSerializer(typeof(float[]));
        FileStream file = File.Create(path);

        xmlSearializer.Serialize(file, cornerCircleCoordinates);
        file.Close();
    }

    private void PositionCircles(float[] positions)
    {
        if (this.cornerCircles != null)
        {
            this.cornerCircles[0].transform.localPosition = new Vector2(positions[0], positions[1]);
            this.cornerCircles[1].transform.localPosition = new Vector2(positions[2], positions[3]);
            this.cornerCircles[2].transform.localPosition = new Vector2(positions[4], positions[5]);
            this.cornerCircles[3].transform.localPosition = new Vector2(positions[6], positions[7]);
        }
    }

    private void PositionCircles(Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft)
    {
        float[] positions = new float[] {topLeft.x, topLeft.y, topRight.x, topRight.y,
                                         bottomRight.x, bottomRight.y, bottomLeft.x, bottomLeft.y };
        PositionCircles(positions);
    }
}
