using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static class ClimbARHandhold
{

    public static UnityEngine.Color ENTERED_COLOR = UnityEngine.Color.green;
    public static UnityEngine.Color RESET_COLOR = UnityEngine.Color.red;

    // holdBoundingBoxes should be transformed to the final "projector" space
    public static GameObject[] InstantiateHandholds(
        GameObject Handhold, // handhold prefab
        Camera camera,
        float[] holdBoundingBoxes)
    {
        float camHeight = 2f * camera.orthographicSize;
        float camWidth = camHeight * camera.aspect;

        int numHolds = holdBoundingBoxes.Length / 4;
        
        List<GameObject> handholds = new List<GameObject>();

        GameObject newHandhold;

        for (int i = 0; i < numHolds; i++)
        {
            int holdIndex = i * 4;
            float x = holdBoundingBoxes[holdIndex] * camWidth - camWidth / 2f;
            float y = holdBoundingBoxes[holdIndex + 1] * camHeight - camHeight / 2f;

            float width = (holdBoundingBoxes[holdIndex + 2] / 2) * camWidth; //divide by 2 because it is a radius
            float height = (holdBoundingBoxes[holdIndex + 3] / 2) * camHeight;

            newHandhold = GameObject.Instantiate(Handhold);
            newHandhold.name = "Handhold " + i;

            Vector2 pos = new Vector2(x + width, (y + height) * -1f);

            if (!StateManager.instance.debugView)
            {
                pos.x = pos.x * -1;
            }
            newHandhold.transform.localPosition = pos;
            newHandhold.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

            Vector3 screenPoint = camera.WorldToViewportPoint(newHandhold.transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

            if (onScreen)
            {
                handholds.Add(newHandhold);
            }
            else
            {
                GameObject.Destroy(newHandhold);
                continue;
            }

            Rigidbody2D rigid = newHandhold.AddComponent<Rigidbody2D>();
            rigid.isKinematic = true;

            CircleCollider2D col = newHandhold.AddComponent<CircleCollider2D>();
            col.radius = (float)Math.Max(width, height);
            col.enabled = true;
            col.isTrigger = true;
            
            Menu.spriteXScale = col.radius / camWidth / 2;
            Menu.spriteYScale = col.radius / camWidth / 2;

            LineRenderer lineRenderer = newHandhold.GetComponent<LineRenderer>();
            lineRenderer.enabled = false;
            DrawBoundingEllipse(lineRenderer, col.radius, col.radius);
       }
        return handholds.ToArray();
    }

    // draw and instantiate custom sprite for climbing hold
    public static void DrawHoldSprite(
        SpriteRenderer spriteRenderer,
        float xscale,
        float yscale)
    {
        spriteRenderer.sprite = Menu.customHoldSprite0;
        spriteRenderer.transform.localScale = new Vector3(xscale, yscale);
        spriteRenderer.enabled = true;
    }

    // draw the bounding ellipse of the climbing hold
    public static void DrawBoundingEllipse(
        LineRenderer lineRenderer,
        float xradius,
        float yradius)
    {
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.cyan;

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

    public static void DestroyChildren(GameObject hold)
    {
        foreach (Transform child in hold.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static void setHoldColor(GameObject hold, UnityEngine.Color color)
    {
        hold.GetComponent<LineRenderer>()
          .startColor = color;
        hold.GetComponent<LineRenderer>()
          .endColor = color;
    }

    public static void HoldLineRendererActive(GameObject hold, bool isActive)
    {
        if (hold != null)
        {
            hold.GetComponent<LineRenderer>().enabled = isActive;
        }
    }

}
