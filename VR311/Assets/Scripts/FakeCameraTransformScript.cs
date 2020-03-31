using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeCameraTransformScript : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, cam.transform.rotation.eulerAngles.z);
    }
}
