using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float mouseX;
    float mouseY;
    public float mouseSenstivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;

    // New bool added by toby, registers if the player is currently locked in an animation like an execution
    public bool turningLocked;
  
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Change 1/1 by Toby, locks camera from moving while bool is true
        if(!turningLocked)
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSenstivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSenstivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
        

    }
}
