using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStateController : MonoBehaviour
{
    public enum Side { Left, Right }
    public Side side;

    public Sprite normalSprite;
    public Sprite doorOpenSprite;
    public Sprite lightOnSprite;
    public Sprite doorOpenLightOnSprite;

    public SpriteRenderer spriteRenderer;

    private bool isDoorOpen = false;
    private bool isLightOn = false;

    // Call this method from the door controller when the door state changes
    public void SetDoorState(bool open)
    {
        isDoorOpen = open;
        UpdateSprite();
    }

    // Call this method from the light controller when the light state changes
    public void SetLightState(bool on)
    {
        isLightOn = on;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (!isDoorOpen && !isLightOn)
        {
            spriteRenderer.sprite = normalSprite; // Door closed, light off
        }
        else if (isDoorOpen && !isLightOn)
        {
            spriteRenderer.sprite = doorOpenSprite; // Door open, light off
        }
        else if (!isDoorOpen && isLightOn)
        {
            spriteRenderer.sprite = lightOnSprite; // Door closed, light on
        }
        else if (isDoorOpen && isLightOn)
        {
            spriteRenderer.sprite = doorOpenLightOnSprite; // Door open, light on
        }
    }
}
