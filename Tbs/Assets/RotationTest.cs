using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    private void OnEnable()
    {
        this.AddObserver(OnCameraChange, CameraRig.CameraRotatingStartNotification);
    }

    void OnCameraChange(object sender, object args)
    {

        var info = (Info<int, float, Vector3>)args;
        int direction = info.arg0;
        float time = info.arg1;
        Vector3 rotationAmount = info.arg2;

        if (direction == 0)
        {
            // Rotate Left
            transform.RotateToLocal(transform.rotation.eulerAngles + rotationAmount, time, EasingEquations.EaseInOutQuad);
        }
        else
        {
            // Rotate Right
            transform.RotateToLocal(transform.rotation.eulerAngles + -rotationAmount, time, EasingEquations.EaseInOutQuad);
        }
    }


}
