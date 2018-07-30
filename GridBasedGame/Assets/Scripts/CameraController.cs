using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float cameraMovementPercentage = 0.01f;
    public int cameraVelocity = 10;

    private float upLimit;
    private float bottomLimit;
    private float leftLimit;
    private float rigthLimit;

    // Use this for initialization
    void Start()
    {
        CalculateCameraMovementZone(cameraMovementPercentage);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.y >= upLimit)
            transform.Translate(Vector3.forward * cameraVelocity * Time.deltaTime);
        if (Input.mousePosition.y <= bottomLimit)
            transform.Translate(Vector3.back * cameraVelocity * Time.deltaTime);
        if (Input.mousePosition.x >= rigthLimit)
            transform.Translate(Vector3.right * cameraVelocity * Time.deltaTime);
        if (Input.mousePosition.x <= leftLimit)
            transform.Translate(Vector3.left * cameraVelocity * Time.deltaTime);
    }

    void CalculateCameraMovementZone(float percentage)
    {
        upLimit = Screen.height - percentage * Screen.height;
        bottomLimit = Screen.height * percentage;
        leftLimit = Screen.width * percentage;
        rigthLimit = Screen.width - percentage * Screen.width;
    }
}
