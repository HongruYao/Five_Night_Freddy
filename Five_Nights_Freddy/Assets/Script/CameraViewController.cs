using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of camera movement
    public float viewShiftAmount = 3f; // Amount of shift when looking left or right

    private Vector3 initialPosition; // Initial center position of the camera
    private Vector3 targetPosition;  // Target position the camera moves towards
    private bool controlEnabled = true; // Track if camera control is enabled

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition; // Start with the initial position
    }

    void Update()
    {
        if (!controlEnabled) return; // Exit if control is disabled

        float screenWidth = Screen.width;

        // Check cursor position and set the target position based on which part of the screen it's in
        if (Input.mousePosition.x < screenWidth * 0.3f) // Left side of the screen
        {
            targetPosition = initialPosition + Vector3.left * viewShiftAmount;
        }
        else if (Input.mousePosition.x > screenWidth * 0.7f) // Right side of the screen
        {
            targetPosition = initialPosition + Vector3.right * viewShiftAmount;
        }
        else // Center of the screen
        {
            targetPosition = initialPosition;
        }

        // Smoothly move towards the target position without abrupt stops
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    // Method to enable or disable camera control
    public void SetControlEnabled(bool enabled)
    {
        controlEnabled = enabled;
        if (!enabled)
        {
            // Reset the camera to the initial position if control is disabled
            transform.position = initialPosition;
        }
    }
}
