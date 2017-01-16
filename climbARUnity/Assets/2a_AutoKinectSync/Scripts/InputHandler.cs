using UnityEngine;
using System.Collections;

// Resizable object logic
// adapted from http://answers.unity3d.com/questions/301951/how-can-i-make-a-gameobject-scaleresize-with-mouse.html
public class InputHandler : MonoBehaviour
{
    //private Ray m_Ray;
    private RaycastHit m_RayCastHit;
    private ResizableObject m_CurrentObject;
    private Vector3 m_LastMousePos;
    private float m_DeltaTime;
    private bool m_AnimateScale;
    private bool m_draggingOut;
    private Vector3 m_StartScale;
    private float m_ScaleFactor;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_LastMousePos = Input.mousePosition;
            //m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // always resize current object
            m_CurrentObject = this.GetComponent<ResizableObject>();
            m_StartScale = m_CurrentObject.transform.localScale;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 deltaPosition = Input.mousePosition - m_LastMousePos;
            if (deltaPosition.magnitude > 1f)
            {
                if (m_CurrentObject && !m_AnimateScale)
                {
                    m_ScaleFactor = deltaPosition.magnitude;
                    m_AnimateScale = true;
                    m_DeltaTime = 0f;
                }
            }
            m_draggingOut = isDraggingOut(m_CurrentObject.transform.position -
                                          m_LastMousePos, deltaPosition);
            m_LastMousePos = Input.mousePosition;
        }

        if (m_AnimateScale && m_DeltaTime < 1f)
        {
            m_DeltaTime += Time.deltaTime;
            if (m_CurrentObject)
            {
                if (!m_draggingOut) // invert scale factor when draggin in
                {
                    m_ScaleFactor = 1 / m_ScaleFactor;
                }

                m_CurrentObject.transform.localScale = Vector3.Lerp(
                    m_CurrentObject.transform.localScale,
                    new Vector3(m_StartScale.x, 0) * m_ScaleFactor,
                    m_DeltaTime);
            }
            m_AnimateScale = false;
            m_DeltaTime = 0f;
        }
    }

    bool isDraggingOut(Vector3 objectVector, Vector3 dragVector)
    {
        return Vector3.Dot(objectVector, dragVector) < 0;
    }
}