using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static class ClimbARUtils
{
    // Converts from image coordinate system ((0,0) is upper left and unbounded range) to unity viewport space ((0,0) is lower left with max height and width of 1) 
    public static Vector2 openCVToUnity(int kinectUpperLeftX, int kinectUpperLeftY, int width, int height, int pointToConvertX, int pointToConvertY)
    {
        float fractionOfX = (pointToConvertX - kinectUpperLeftX) / (width);
        float fractionOfY = ((kinectUpperLeftY + height) - pointToConvertY) / (height);

        return new Vector2(fractionOfX, fractionOfY);
    }

    // Converts an array of opencv boxes (upper left x,y and then width and height) to array of unity coord boxes
    public static float[] convertOpenCVBoxesToUnityBoxes(int kinectUpperLeftX, int kinectUpperLeftY, int width, int height, int[] coords)
    {
        float[] transformed = new float[coords.Length];

        for (int i = 0; i < coords.Length / 4; i++)
        {
            int holdIdx = i * 4;
            Vector2 convertedCorner = openCVToUnity(kinectUpperLeftX, kinectUpperLeftY, width, height, coords[holdIdx], coords[holdIdx + 1]);
            transformed[holdIdx] = convertedCorner.x;
            transformed[holdIdx + 1] = convertedCorner.y;
            transformed[holdIdx + 2] = coords[holdIdx + 2] / width;
            transformed[holdIdx + 3] = coords[holdIdx + 3] / height;
        }

        return transformed;
    }

    // take a point x and y in [0,1] and translate to the camera space (orthographic)
    public static Vector2 fractionToCameraSpace(float x, float y, Camera cam)
    {
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        return new Vector2(x * camWidth - camWidth / 2f,
                           y * camHeight - camHeight / 2f);
    }
}
