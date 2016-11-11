using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class SpaceUtils
{

    static int kinectMinX, kinectMinY, kinectMaxX, kinectMaxY;
    // Kinect coords representing the min/max of the 2d canvas

    // Function to convert x,y pixel values ((0,0) being upper left) to 2 dimensional viewport ((0,0) being lower left with scale of 1,1)

    // These are then converted to world space ((x,y,z) from lower left) and possibly all having the same depth (skeleton might use z coords to adjust size)

    void convertOpenCVCoordToUnityCoord(int openX, int openY)
    {
        // If
        if (openX > kinectMaxX || openX < kinectMinX || openY > kinectMaxY || openY < kinectMinY)
        {
            return
        }
        // Find how close point is to 
    }

    public void setKinectCoords(int minX, int minY, int maxX, int maxY)
    {
        kinectMinX = minX;
        kinectMinY = minY;
        kinectMaxX = maxX;
        kinectMaxY = maxY;
    }


    private float[] transformOpenCvToUnitySpace(int[] coordinates, int[] boundingBoxArray)
    {
        int x1 = coordinates[0];
        int y1 = coordinates[1];
        int x2 = coordinates[2];
        int y2 = coordinates[3];
        int x3 = coordinates[4];
        int y3 = coordinates[5];
        int x4 = coordinates[6];
        int y4 = coordinates[7];

        float[] transformedArr = new float[this.numHolds * 4];

        float height = y4 - y1; //this is assuming y1 and y2 are approximately the same

        float leftGradient = (x4 - x1) / height;
        float rightGradient = (x3 - x2) / (y3 - y2);

        for (int i = 0; i < this.numHolds; i++)
        {
            int holdIndex = i * 4;

            // get coordinates of hold
            int currentX = boundingBoxArray[holdIndex];
            int currentY = boundingBoxArray[holdIndex + 1];
            int holdWidth = boundingBoxArray[holdIndex + 2];
            int holdHeight = boundingBoxArray[holdIndex + 3];

            //Project y on bb side left to get coordinates of the beginning of the horizonal line on which this hold belongs
            float leftX = x1 + leftGradient * (currentY - y1);

            //Project y on bb side right to get coordinates of the end of the horizonal line on which this hold belongs
            float rightX = x2 + rightGradient * (currentY - y2);

            //get length of corresponding horizontal line
            float xLength = rightX - leftX;

            //save values
            transformedArr.SetValue((currentX - leftX) / xLength, holdIndex);
            transformedArr.SetValue((currentY - y1) / height, holdIndex + 1);
            transformedArr.SetValue(holdWidth / xLength, holdIndex + 2);
            transformedArr.SetValue(holdHeight / height, holdIndex + 3);
        }

        return transformedArr;
    }
}
