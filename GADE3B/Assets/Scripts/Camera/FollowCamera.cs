using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform target; // The object the camera will focus on (e.g., the main tower)
    public Vector3 offset = new Vector3(0, 10, -10);
    public float smoothSpeed = 0.125f;

    public float minZoom = 5f;  // Minimum zoom level
    public float maxZoom = 20f; // Maximum zoom level
    public float zoomSpeed = 4f;
    private float currentZoom = 10f;

    public float rotationSpeed = 100f; // Speed of the camera rotation
    private float currentRotation = 0f; // Track the current rotation angle

    // Method to set the target for the camera
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        HandleZoom();
        HandleRotation();
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Quaternion rotation = Quaternion.Euler(0f, currentRotation, 0f);
            Vector3 desiredPosition = target.position + rotation * offset * currentZoom;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(target);
        }
    }

    void HandleZoom()
    {
        // Get the scroll input
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Modify the current zoom based on scroll input
        currentZoom -= scroll * zoomSpeed;

        // Clamp the zoom value between minZoom and maxZoom
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    void HandleRotation()
    {
        // Rotate the camera when the player holds the right mouse button
        if (Input.GetMouseButton(1))
        {
            float rotationInput = Input.GetAxis("Mouse X"); // Horizontal mouse movement
            currentRotation += rotationInput * rotationSpeed * Time.deltaTime;
        }
    }
}
