using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceView : MonoBehaviour 
{
    public Material BoneMaterial;
    public GameObject BodySourceManager;
    private Kinect.KinectSensor _Sensor;
    public Camera mainCam;

    public GameObject Bone;

    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;
    
    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };

    void Start ()
    {
        _Sensor = Kinect.KinectSensor.GetDefault();
    }
    
    void Update () 
    {
        if (BodySourceManager == null)
        {
            return;
        }
        
        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }
        
        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }
        
        List<ulong> trackedIds = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
              }
                
            if(body.IsTracked)
            {
                trackedIds.Add (body.TrackingId);
            }
        }
        
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
        
        // First delete untracked bodies
        foreach(ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                DestroyCollidersAttachedToBody(_Bodies[trackingId]);
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
            }
            
            if(body.IsTracked)
            {
                if(!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }
                
                RefreshBodyObject(body, _Bodies[body.TrackingId]);
            }
        }
    }
    
    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            DestroyImmediate(jointObj.GetComponent("BoxCollider"));

            if (jt == Kinect.JointType.HandLeft || jt == Kinect.JointType.HandRight || 
                jt == Kinect.JointType.HandTipLeft || jt == Kinect.JointType.HandTipRight || 
                jt == Kinect.JointType.ThumbLeft || jt == Kinect.JointType.ThumbRight)
            {
                
                Rigidbody2D rigid = jointObj.AddComponent<Rigidbody2D>();
                rigid.isKinematic = true;
                CircleCollider2D col = jointObj.AddComponent<CircleCollider2D>();
                col.enabled = true;
                col.isTrigger = true;
            }

            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.05f, 0.05f);
            
            jointObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }
        
        return body;
    }
    
    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;

            if(_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }
            
            Vector3 position = GetVector3FromJoint(sourceJoint);
            if (float.IsInfinity(position.x) || float.IsNegativeInfinity(position.x) ||
                float.IsInfinity(position.y) || float.IsNegativeInfinity(position.y))
            {
                continue;
            }

            Transform jointObj = bodyObject.transform.FindChild(jt.ToString());
            jointObj.localPosition = position;
            
            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            if(targetJoint.HasValue)
            {
                if (jt == Kinect.JointType.HandLeft || jt == Kinect.JointType.HandRight ||
                    jt == Kinect.JointType.HandTipLeft || jt == Kinect.JointType.HandTipRight ||
                    jt == Kinect.JointType.ThumbLeft || jt == Kinect.JointType.ThumbRight)
                {
                    CircleCollider2D col = jointObj.gameObject.GetComponent<CircleCollider2D>();
                    if (col != null)
                    {
                        col.radius = 1.0f;
                        //col.offset = new Vector2(jointObj.localPosition.x, jointObj.localPosition.y); 
                    } 
                }
                lr.SetPosition(0, jointObj.localPosition);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
                lr.SetColors(GetColorForState (sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
            }
            else
            {
                lr.enabled = false;
            }
        }
    }

    private void DestroyCollidersAttachedToBody(GameObject bodyObject)
    {
        Transform leftHand = bodyObject.transform.FindChild(Kinect.JointType.HandLeft.ToString());
        CircleCollider2D leftCol = leftHand.gameObject.GetComponent<CircleCollider2D>();

        if (leftCol)
        {
            DestroyImmediate(leftCol);
        }

        Transform rightHand = bodyObject.transform.FindChild(Kinect.JointType.HandRight.ToString());
        CircleCollider2D rightCol = rightHand.gameObject.GetComponent<CircleCollider2D>();

        if (rightCol)
        {
            DestroyImmediate(rightCol);
        }
    }
    
    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
        case Kinect.TrackingState.Tracked:
            return Color.green;

        case Kinect.TrackingState.Inferred:
            return Color.red;

        default:
            return Color.black;
        }
    }
    
    private Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        Kinect.CameraSpacePoint cameraPoint = joint.Position;

        return GetUnitySpaceFromKinectCameraPoint(cameraPoint, 0, this.mainCam, 1920, 1080);
    }

    private Vector3 GetUnitySpaceFromKinectCameraPoint(Kinect.CameraSpacePoint point, float depth, Camera cam, int kinectWidth, int kinectHeight)
    {
        Kinect.ColorSpacePoint colorPoint = _Sensor.CoordinateMapper.MapCameraPointToColorSpace(point);
        float newX = ((2 * colorPoint.X - kinectWidth) / kinectWidth) * (cam.orthographicSize * cam.aspect);
        float newY = ((kinectHeight - 2 * colorPoint.Y) / kinectHeight) * cam.orthographicSize;
        
        return new Vector3(newX, newY, depth);
    }
}
