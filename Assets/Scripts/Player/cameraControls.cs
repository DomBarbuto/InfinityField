using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{

    //Controls the sensitivity of the camera
    [SerializeField] int sensHor;
    [SerializeField] int sensVer;

    //Controls the Clamping of the camera
    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;

    //incase the player wants the x inverted
    [SerializeField] bool invertX;

    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Get the input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVer;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;

        //X Rotation of the Camera
        if (invertX)
            xRotation += mouseY;
        else
            xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax); //Clamp Camera

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0); //Rotate Camera on the X-Axis

        //Rotate Player on the Y-Axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
