using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    public float m_followSpeed = 3.0f;
    public Transform m_follow; // Target

    public Transform m_heading;
    public Transform m_pitch;

    public float m_rotationTime = 1f;


    Transform _transform;
    Tweener rotation;

    Vector3 rotationValue = new Vector3(0, 90, 0);


    public const string CameraRotatingStartNotification = "CameraRig.RotationStart";
    //public const string CameraRotationCompleteNotificaiton = "CameraRig.RotationComplete";

    // Which direciton the camera is presently facing.
    
    public static Directions Forward
    {
        get
        {
            return forward;
        }
    }
    static Directions forward;
    

    
    


    private void Awake()
    {
        _transform = transform;
        forward = Directions.North;
    }

    private void Update()
    {
        if(m_follow)
        {
            // AHH THIS USE OF LERP ALWAYS MAKES ME MAD.
            // Never reaching goal.
            // Means can never check if you've hit the target or not.
            _transform.position = Vector3.Lerp(_transform.position, m_follow.position, m_followSpeed * Time.deltaTime);
        }
    }

    // Should pass in an input key.
    public void CombatRotate(int direction)
    {
        if (rotation == null || !rotation.IsPlaying)
        {
            direction = direction == 0 ? -1 : 1;
            adjustCameraFacing(direction);


            // No notify any assets that rotation is occuring.
            this.PostNotification(CameraRotatingStartNotification, new Info<int, float, Vector3>(direction, m_rotationTime, rotationValue));

            rotation = m_heading.RotateToLocal(m_heading.rotation.eulerAngles + (-direction * rotationValue), m_rotationTime, EasingEquations.EaseInOutQuad);

        }
    }

    void adjustCameraFacing(int direction)
    {
        forward += direction;
        Debug.Log(forward);
        if ((int)forward > 3)
            forward = Directions.North;
        else if ((int)forward < 0)
            forward = Directions.West;
        //Debug.Log(forward);
    }

}
