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

        transform.RotateToLocal(transform.rotation.eulerAngles + -(direction * rotationAmount), time, EasingEquations.EaseInOutQuad);
    }


}
