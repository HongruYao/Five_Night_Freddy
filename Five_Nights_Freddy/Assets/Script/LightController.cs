using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Sprite normalBackground;        // Normal background sprite
    public Sprite leftLightOnSprite;       // Left light on sprite
    public Sprite rightLightOnSprite;      // Right light on sprite
    public SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component

    private bool isLeftLightOn = false;    // Track if left light is on
    private bool isRightLightOn = false;   // Track if right light is on

    public ButtonStateController leftButtonStateController; // Reference to left button state controller
    public ButtonStateController rightButtonStateController; // Reference to right button state controller
    public PowerController powerController; // Reference to the PowerController

    // Call this when left light button is clicked
    public void ToggleLeftLight()
    {
        if (isLeftLightOn)
        {
            // Turn off left light if it's already on
            spriteRenderer.sprite = normalBackground;
            isLeftLightOn = false;
            leftButtonStateController.SetLightState(false);
            powerController.ToggleSystem(false); // Inform power system to decrease usage
        }
        else
        {
            // Turn on left light and turn off right light if it's on
            spriteRenderer.sprite = leftLightOnSprite;
            isLeftLightOn = true;
            if (isRightLightOn) // If right light is on, turn it off
            {
                isRightLightOn = false;
                rightButtonStateController.SetLightState(false);
                powerController.ToggleSystem(false); // Reduce usage for turning off the right light
            }
            leftButtonStateController.SetLightState(true);
            powerController.ToggleSystem(true); // Inform power system to increase usage
        }
    }

    // Call this when right light button is clicked
    public void ToggleRightLight()
    {
        if (isRightLightOn)
        {
            // Turn off right light if it's already on
            spriteRenderer.sprite = normalBackground;
            isRightLightOn = false;
            rightButtonStateController.SetLightState(false);
            powerController.ToggleSystem(false); // Inform power system to decrease usage
        }
        else
        {
            // Turn on right light and turn off left light if it's on
            spriteRenderer.sprite = rightLightOnSprite;
            isRightLightOn = true;
            if (isLeftLightOn) // If left light is on, turn it off
            {
                isLeftLightOn = false;
                leftButtonStateController.SetLightState(false);
                powerController.ToggleSystem(false); // Reduce usage for turning off the left light
            }
            rightButtonStateController.SetLightState(true);
            powerController.ToggleSystem(true); // Inform power system to increase usage
        }
    }

    // Method to turn off both lights
    public void TurnOffLights()
    {
        // Set both lights to "off" and reset to normal background
        if (isLeftLightOn) powerController.ToggleSystem(false);
        if (isRightLightOn) powerController.ToggleSystem(false);

        isLeftLightOn = false;
        isRightLightOn = false;
        spriteRenderer.sprite = normalBackground;

        // Update the button states to reflect lights off
        leftButtonStateController.SetLightState(false);
        rightButtonStateController.SetLightState(false);
    }
}
