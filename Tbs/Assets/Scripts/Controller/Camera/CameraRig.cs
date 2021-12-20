using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    public float m_speed = 3.0f;
    public Transform m_follow; // Target

    Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if(m_follow)
        {
            // AHH THIS USE OF LERP ALWAYS MAKES ME MAD.
            // Never reaching goal.
            // Means can never check if you've hit the target or not.
            _transform.position = Vector3.Lerp(_transform.position, m_follow.position, m_speed * Time.deltaTime);
        }
    }
}
