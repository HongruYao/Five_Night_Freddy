using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorController : MonoBehaviour
{
    public List<Sprite> monitorSprites; // List to hold 11 sprites for the monitor animation
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public GameObject monitorPanel; // Reference to the panel that will be shown/hidden
    public float animationDuration = 1.5f; // Total duration for the animation

    public LightController lightController; // Reference to the LightController
    public PowerController powerController; // Reference to the PowerController
    public CameraViewController cameraController; // Reference to the CameraViewController
    private bool isMonitorOpen = false; // Track if the monitor is open
    private Coroutine currentAnimation; // Reference to the current animation coroutine

    void Start()
    {
        monitorPanel.SetActive(false); // Start with the panel hidden
        spriteRenderer.enabled = false; // Disable SpriteRenderer initially
    }

    public void ToggleMonitor()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        if (isMonitorOpen)
        {
            // Hide the panel, disable SpriteRenderer, and play animation backward
            monitorPanel.SetActive(false);
            currentAnimation = StartCoroutine(PlayAnimation(monitorSprites.Count - 1, 0, hideSpriteAfter: true));
            isMonitorOpen = false;
            powerController.ToggleSystem(false); // Decrease usage when monitor is closed
            cameraController.SetControlEnabled(true); // Re-enable camera control
        }
        else
        {
            // Turn off lights when monitor is activated
            lightController.TurnOffLights();

            // Enable SpriteRenderer, play animation forward, and show the panel after animation completes
            spriteRenderer.enabled = true;
            currentAnimation = StartCoroutine(PlayAnimation(0, monitorSprites.Count - 1, showPanelAfter: true));
            isMonitorOpen = true;
            powerController.ToggleSystem(true); // Increase usage when monitor is opened
            cameraController.SetControlEnabled(false); // Disable camera control
        }
    }

    private IEnumerator PlayAnimation(int start, int end, bool showPanelAfter = false, bool hideSpriteAfter = false)
    {
        int frameCount = monitorSprites.Count;
        float frameTime = animationDuration / frameCount;
        int step = (start < end) ? 1 : -1; // Determine direction of animation

        for (int i = start; i != end + step; i += step)
        {
            spriteRenderer.sprite = monitorSprites[i];
            yield return new WaitForSeconds(frameTime);
        }

        // Show the panel if specified (only for opening animation)
        if (showPanelAfter)
        {
            monitorPanel.SetActive(true);
        }

        // Hide the SpriteRenderer after the backward animation completes
        if (hideSpriteAfter)
        {
            spriteRenderer.enabled = false;
        }
    }
}
