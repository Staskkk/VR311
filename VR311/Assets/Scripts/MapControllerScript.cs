using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControllerScript : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float zoomSpeed;
    [SerializeField]
    float rotSensitivity;

    public Transform camTransform;
    public Transform fakeCameraTransform;

    private bool isRotating;
    private float anchorPoint;
    private Quaternion anchorRot;

    void Update()
    {
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            move += Vector3.forward * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move -= Vector3.forward * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += Vector3.right * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move -= Vector3.right * speed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            move += Vector3.up * speed;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            move -= Vector3.up * speed;
        }

        transform.Translate(move * Time.deltaTime, fakeCameraTransform);
        transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime, camTransform);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit && hitInfo.transform.CompareTag("Map"))
            {
                anchorPoint = Input.mousePosition.x;
                anchorRot = transform.rotation;
                isRotating = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }

        if (isRotating)
        {
            Quaternion rot = anchorRot;
            float dif = anchorPoint - Input.mousePosition.x;
            rot.eulerAngles += new Vector3(0, dif * rotSensitivity, 0);
            transform.rotation = rot;
        }
    }
}
