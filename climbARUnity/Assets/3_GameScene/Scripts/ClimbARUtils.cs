using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ViewCoords
{
    public float X;
    public float Y;

    public ViewCoords(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }
}

static class ClimbARUtils
{
    // Converts from image coordinate system ((0,0) is upper left and unbounded range) to unity viewport space ((0,0) is lower left with max height and width of 1) 
    public static ViewCoords openCVToUnity(int kinectUpperLeftX, int kinectUpperLeftY, int width, int height, int pointToConvertX, int pointToConvertY)
    {
        float fractionOfX = (pointToConvertX - kinectUpperLeftX) / (width);
        float fractionOfY = ((kinectUpperLeftY + height) - pointToConvertY) / (height);

        return new ViewCoords(fractionOfX, fractionOfY);
    }

    // Converts an array of opencv boxes (upper left x,y and then width and height) to array of unity coord boxes
    public static float[] convertOpenCVBoxesToUnityBoxes(int kinectUpperLeftX, int kinectUpperLeftY, int width, int height, int[] coords)
    {
        float[] transformed = new float[coords.Length];

        for (int i = 0; i < coords.Length / 4; i++)
        {
            int holdIdx = i * 4;
            ViewCoords convertedCorner = openCVToUnity(kinectUpperLeftX, kinectUpperLeftY, width, height, coords[holdIdx], coords[holdIdx + 1]);
            transformed[holdIdx] = convertedCorner.X;
            transformed[holdIdx + 1] = convertedCorner.Y;
            transformed[holdIdx + 2] = coords[holdIdx + 2] / width;
            transformed[holdIdx + 3] = coords[holdIdx + 3] / height;

        }

        return transformed;
    }
}
