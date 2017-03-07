using System;
using UnityEngine;
using System.Collections;


public static class RouteGeneration
{


    //----------------------------------------------------
    // TODO: Test these functions!!!

    // also,do we need to pass in handHolds[]?
    //----------------------------------------------------

    static float getDistanceBetweenHolds(GameObject hold1, GameObject hold2)
    {
        float x1, x2, y1, y2;
        x1 = hold1.transform.position.x;
        y1 = hold1.transform.position.y;
        x2 = hold2.transform.position.x;
        y2 = hold2.transform.position.y;

        return Mathf.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
    }

    public static GameObject getNearestHold(GameObject[] handHolds, GameObject currentHold)
    {
        // input tests i
        if (handHolds == null)
        {
            Console.Error.WriteLine("Handholds array is null!");
            return null;
        }
        if (handHolds.Length == 0)
        {
            Console.Error.WriteLine("Handholds array is empty");
            return null;
        }


        float minDistance = float.MaxValue;
        GameObject minHold = null;
        for (int i = 0; i < handHolds.Length; i++)
        {
            if (handHolds[i] != currentHold && getDistanceBetweenHolds(currentHold, handHolds[i]) < minDistance)
            {
                minDistance = getDistanceBetweenHolds(currentHold, handHolds[i]);
                minHold = handHolds[i];
            }
        }

        return minHold;
    }


    public static GameObject getNearestHoldAbove(GameObject[] handHolds, GameObject currentHold)
    {
        float minDistance = float.MaxValue;
        GameObject minHold = null;
        for (int i = 0; i < handHolds.Length; i++)
        {
            if (handHolds[i] != currentHold &&
                handHolds[i].transform.position.y > currentHold.transform.position.y &&
                getDistanceBetweenHolds(currentHold, handHolds[i]) < minDistance)
            {
                minDistance = getDistanceBetweenHolds(currentHold, handHolds[i]);
                minHold = handHolds[i];
            }
        }

        return minHold;
    }

    public static GameObject getNearestHoldBelow(GameObject[] handHolds, GameObject currentHold)
    {
        float minDistance = float.MaxValue;
        GameObject minHold = null;
        for (int i = 0; i < handHolds.Length; i++)
        {
            if (handHolds[i] != currentHold &&
                handHolds[i].transform.position.y < currentHold.transform.position.y &&
                getDistanceBetweenHolds(currentHold, handHolds[i]) < minDistance)
            {
                minDistance = getDistanceBetweenHolds(currentHold, handHolds[i]);
                minHold = handHolds[i];
            }
        }

        return minHold;
    }

    public static GameObject getNearestHoldRight(GameObject[] handHolds, GameObject currentHold)
    {
        float minDistance = float.MaxValue;
        GameObject minHold = null;
        for (int i = 0; i < handHolds.Length; i++)
        {
            if (handHolds[i] != currentHold &&
                handHolds[i].transform.position.x > currentHold.transform.position.x &&
                getDistanceBetweenHolds(currentHold, handHolds[i]) < minDistance)
            {
                minDistance = getDistanceBetweenHolds(currentHold, handHolds[i]);
                minHold = handHolds[i];
            }
        }

        return minHold;
    }

    public static GameObject getNearestHoldLeft(GameObject[] handHolds, GameObject currentHold)
    {
        float minDistance = float.MaxValue;
        GameObject minHold = null;
        for (int i = 0; i < handHolds.Length; i++)
        {
            if (handHolds[i] != currentHold &&
                handHolds[i].transform.position.x < currentHold.transform.position.x &&
                getDistanceBetweenHolds(currentHold, handHolds[i]) < minDistance)
            {
                minDistance = getDistanceBetweenHolds(currentHold, handHolds[i]);
                minHold = handHolds[i];
            }
        }

        return minHold;
    }

    // returns a list of game objects ordered from lowest to highest
    public static ArrayList generateRandomRoute(GameObject[] handHolds)
    {
        GameObject currentHold;
        ArrayList routeHolds = new ArrayList();
        currentHold = getStartingHold(handHolds);

        while (currentHold != null)
        {
            routeHolds.Add(currentHold);
            currentHold = getNearestHoldAbove(handHolds, currentHold);
        }

        return routeHolds;
    }

    public static GameObject getStartingHold(GameObject[] handHolds)
    {
        GameObject[] sortedArray = new GameObject[handHolds.Length];
        Array.Copy(handHolds, sortedArray, handHolds.Length);
        Array.Sort(sortedArray, delegate (GameObject g1, GameObject g2) { return g1.transform.position.y.CompareTo(g2.transform.position.y); });

        //get random index
        System.Random r = new System.Random();
        int index = r.Next(0, sortedArray.Length / 4); //start route from lower quarter of holds

        sortedArray[index].GetComponent<LineRenderer>().startColor = UnityEngine.Color.blue;
        sortedArray[index].GetComponent<LineRenderer>().endColor = UnityEngine.Color.blue;

        return sortedArray[index]; //to be changed

    }
}
