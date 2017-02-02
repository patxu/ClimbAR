using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGameDriver : MonoBehaviour
{
    public Camera mainCam;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            mainCam.cullingMask = ~(1 << 8);
        }
        else if (Input.GetKeyDown("s"))
        {
            mainCam.cullingMask = 0xffff;
        }
    }
}
