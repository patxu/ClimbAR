﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static class ClimbARHandhold
{
    // holdBoundingBoxes should be transformed to the correct space
    public static GameObject[] InstantiateHandholds(
        GameObject Handhold, // handhold prefab
        int numHolds, // can we just use holdBoundingBoxes?
        Camera camera,
        float[] holdBoundingBoxes)
    {
        float camHeight = 2f * camera.orthographicSize;
        float camWidth = camHeight * camera.aspect;

        GameObject[] handholds = new GameObject[numHolds];

        for (int i = 0; i < numHolds; i++)
        {
            int holdIndex = i * 4;
            float x = holdBoundingBoxes[holdIndex] * camWidth - camWidth / 2f;
            float y = holdBoundingBoxes[holdIndex + 1] * camHeight - camHeight / 2f;

            float width = (holdBoundingBoxes[holdIndex + 2] / 2) * camWidth; //divide by 2 because it is a radius
            float height = (holdBoundingBoxes[holdIndex + 3] / 2) * camHeight;

            handholds[i] = GameObject.Instantiate(Handhold);
            handholds[i].name = "Handhold " + i;

            Vector2 pos = new Vector2(x + width, (y + height) * -1f);

            if (!StateManager.instance.debugView)
            {
                pos.x = pos.x * -1;
            }
            handholds[i].transform.localPosition = pos;
            handholds[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

            Rigidbody2D rigid = handholds[i].AddComponent<Rigidbody2D>();
            rigid.isKinematic = true;

            CircleCollider2D col = handholds[i].AddComponent<CircleCollider2D>();
            col.radius = (float)Math.Max(width, height);
            col.enabled = true;
            col.isTrigger = true;

            // Create handhold object and draw bounding ellipse
            LineRenderer lineRenderer = handholds[i].GetComponent<LineRenderer>();
            DrawBoundingEllipse(lineRenderer, width, height);
        }
        return handholds;
    }

    // draw the bounding ellipse of the climbing hold
    private static void DrawBoundingEllipse(
        LineRenderer lineRenderer,
        float xradius,
        float yradius)
    {
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        float x;
        float y;

        // resolution of the sides of the ellipse
        int segments = 50;
        lineRenderer.numPositions = segments + 2;

        // width of line; scaled by width and height of bounding box
        //float lineWidth = Math.Min(xradius, yradius) / 5f;
        float lineWidth = 0.05f;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // not currently setting the angle of ellipse
        float angle = 0f;

        for (int i = 0; i < (segments + 2); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }
    }


}
