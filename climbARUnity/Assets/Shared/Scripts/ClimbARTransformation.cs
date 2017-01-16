using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static class ClimbARTransformation
{

    public static float[] transformOpenCvToUnitySpace(
        float[] coordinates,
        float[] boundingBoxArray)
    {
        float x1 = coordinates[0];
        float y1 = coordinates[1];
        float x2 = coordinates[2];
        float y2 = coordinates[3];
        float x3 = coordinates[4];
        float y3 = coordinates[5];
        float x4 = coordinates[6];
        float y4 = coordinates[7];

        float[] transformedArr = new float[boundingBoxArray.Length];

        float height = y4 - y1; //this is assuming y1 and y2 are approximately the same

        float leftGradient = (x4 - x1) / height;
        float rightGradient = (x3 - x2) / (y3 - y2);

        for (int i = 0; i < boundingBoxArray.Length / 4; i++)
        {
            //Debug.Log("Hold: ");
            int holdIndex = i * 4;

            // get coordinates of hold
            float currentX = boundingBoxArray[holdIndex];
            float currentY = boundingBoxArray[holdIndex + 1];
            float holdWidth = boundingBoxArray[holdIndex + 2];
            float holdHeight = boundingBoxArray[holdIndex + 3];

            // Project y on bb side left to get coordinates of the beginning of 
            // the horizonal line on which this hold belongs
            float leftX = x1 + leftGradient * (currentY - y1);

            // Project y on bb side right to get coordinates of the end of the 
            // horizonal line on which this hold belongs
            float rightX = x2 + rightGradient * (currentY - y2);

            //get length of corresponding horizontal line
            float xLength = rightX - leftX;

            transformedArr.SetValue((currentX - leftX) / xLength, holdIndex);
            transformedArr.SetValue((currentY - y1) / height, holdIndex + 1);
            transformedArr.SetValue(holdWidth / xLength, holdIndex + 2);
            transformedArr.SetValue(holdHeight / height, holdIndex + 3);
        }

        return transformedArr;
    }
}
