using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public enum DoorSide { Left, Right }
    public DoorSide doorSide; // Specify if this is the left or right door

    public List<Sprite> doorSprites; // List to hold 17 sprites for the door animation
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public float animationDuration = 2f; // Duration for the door animation

    public bool isOpen = false; // Track if the door is open or closed
    private bool isAnimating = false; // Track if an animation is currently running
    private Coroutine currentAnimation; // Reference to the current animation coroutine

    public ButtonStateController leftButtonStateController;
    public ButtonStateController rightButtonStateController;
    public PowerController powerController; // Reference to PowerController

    void Start()
    {
        spriteRenderer.sprite = doorSprites[0]; // Start with the first sprite in the list
    }

    public void ToggleDoor()
    {
        if (isAnimating) return;

        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        // Check the current door state and play the appropriate animation
        if (isOpen)
        {
            currentAnimation = StartCoroutine(AnimateDoor(doorSprites.Count - 1, 0));
            powerController.ToggleSystem(false); // Decrease usage when closing the door
        }
        else
        {
            currentAnimation = StartCoroutine(AnimateDoor(0, doorSprites.Count - 1));
            powerController.ToggleSystem(true); // Increase usage when opening the door
        }

        isOpen = !isOpen; // Toggle door state

        // Update button state based on door side
        if (doorSide == DoorSide.Left)
        {
            leftButtonStateController.SetDoorState(isOpen);
        }
        else if (doorSide == DoorSide.Right)
        {
            rightButtonStateController.SetDoorState(isOpen);
        }
    }

    private IEnumerator AnimateDoor(int start, int end)
    {
        isAnimating = true; // Set flag to true when animation starts

        int frameCount = doorSprites.Count;
        float frameTime = animationDuration / frameCount;
        int step = (start < end) ? 1 : -1; // Determine direction

        for (int i = start; i != end + step; i += step)
        {
            spriteRenderer.sprite = doorSprites[i];
            yield return new WaitForSeconds(frameTime);
        }

        isAnimating = false; // Reset flag when animation finishes
    }
}
