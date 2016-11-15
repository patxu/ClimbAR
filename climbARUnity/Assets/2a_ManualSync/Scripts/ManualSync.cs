using UnityEngine;
using System.Collections;

public class ManualSync : MonoBehaviour
{

    // Game objects
    public GameObject[] cornerCircles;
    public GameObject CornerCircle;
    public Camera mainCam;

    // Use this for initialization
    void Start()
    {
        InitCornerCircles();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Top left, top right, bottom right, bottom left
    /// </summary>
    void InitCornerCircles()
    {
        this.cornerCircles = new GameObject[4];
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                this.cornerCircles[i * 2 + j] = GameObject.Instantiate(CornerCircle);
                this.cornerCircles[i * 2 + j].name = "CornerCircle " + (i * 2 + j);
            }
        }
        this.cornerCircles[0].transform.localPosition = ClimbARUtils.fractionToCameraSpace(0, 1, this.mainCam);
        this.cornerCircles[1].transform.localPosition = ClimbARUtils.fractionToCameraSpace(1, 1, this.mainCam);
        this.cornerCircles[2].transform.localPosition = ClimbARUtils.fractionToCameraSpace(1, 0, this.mainCam);
        this.cornerCircles[3].transform.localPosition = ClimbARUtils.fractionToCameraSpace(0, 0, this.mainCam);
    }
}
