using UnityEngine;
using System.Collections;

public class CornerCircleCollider : MonoBehaviour
{
    Vector3 offset;
    void OnMouseDown()
    {
        Vector3 mousePos = Input.mousePosition;
        offset = this.transform.position - Camera.main.ScreenToWorldPoint(mousePos);
    }

    void OnMouseDrag()
    {
        Vector3 mousePos = Input.mousePosition;
        this.transform.position =
            Camera.main.ScreenToWorldPoint(mousePos) + offset;
    }
}
