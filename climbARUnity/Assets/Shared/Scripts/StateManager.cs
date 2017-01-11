using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour
{

    // Variables relating to state
    public Vector2 kinectUpperLeft, kinectUpperRight, kinectLowerLeft, kinectLowerRight;
    public bool debugView = true;

    public static StateManager instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // scale coordinates of projector bounds and return as int array
    public float[] getProjectorBounds()
    {
        Vector2 topLeft = StateManager.instance.kinectUpperLeft; 
        topLeft.Scale(ClimbARUtils.kinectScale);
        Vector2 topRight = StateManager.instance.kinectUpperRight; 
        topRight.Scale(ClimbARUtils.kinectScale);
        Vector2 bottomRight = StateManager.instance.kinectLowerRight; ;
        bottomRight.Scale(ClimbARUtils.kinectScale);
        Vector2 bottomLeft = StateManager.instance.kinectLowerLeft; 
        bottomLeft.Scale(ClimbARUtils.kinectScale);
        return new float[] { topLeft.x, topLeft.y, topRight.x, topRight.y, bottomRight.x, bottomRight.y, bottomLeft.x, bottomLeft.y };
    }
}
