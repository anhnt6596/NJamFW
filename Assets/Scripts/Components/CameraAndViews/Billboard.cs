using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void OnEnable()
    {
        DoBillboard();
    }
    private void LateUpdate()
    {
        if (CameraViewDir.TransformChanged)
            DoBillboard();
    }

    public void DoBillboard()
    {
        transform.forward = CameraViewDir.CamForward;
    }
}
