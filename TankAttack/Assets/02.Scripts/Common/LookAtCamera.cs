using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform CamTr;
    Transform CanvasTr;

    void Start()
    {
        CamTr = Camera.main.transform;
        CanvasTr = transform;
    }

    void Update()
    {
        CanvasTr.LookAt(CamTr);
    }
}
