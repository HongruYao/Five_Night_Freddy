using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonitorPanelController : MonoBehaviour
{
    [System.Serializable]
    public class ButtonInfo
    {
        public string buttonName; // Button name for reference
        public Button button; // Button component
        public Sprite image; // Corresponding image for the button
        public string displayName; // Corresponding display name
    }

    public List<ButtonInfo> buttonInfos; // List of all button information
    public Image displayImage; // Image component to show the selected button image
    public TextMeshProUGUI displayText; // Text to show the specific name
    public Image redDotImage; // Reference to the red dot image for recording sign

    public ButtonInfo activeButtonInfo; // Currently active button info
    private Coroutine highlightCoroutine; // Reference to the current highlight coroutine
    private Coroutine movementCoroutine; // Reference to the movement coroutine
    private Coroutine redDotCoroutine; // Reference to the red dot blinking coroutine

    private Color normalColor = new Color32(0x5A, 0x5A, 0x5A, 0xFF); // Default color (hex: #5A5A5A)
    private Color highlightColor = new Color32(0x1A, 0xB7, 0x15, 0xFF); // Highlight color (hex: #1AB715)

    public float movementDistance = 50f; // Distance the image moves to the right
    public float movementSpeed = 10f; // Speed of the image movement
    public float pauseDuration = 2f; // Time to pause after moving
    public string camera6ButtonName = "CAM6"; // Button name for Camera 6
    public Vector3 nonCamera6StartPosition = new Vector3(-304f, 0f, 0f); // Starting position for non-Camera 6 views

    void Start()
    {
        // Assign click event listeners for each button
        foreach (var buttonInfo in buttonInfos)
        {
            ButtonInfo localButtonInfo = buttonInfo; // Local reference for closure
            buttonInfo.button.onClick.AddListener(() => OnButtonClicked(localButtonInfo));
        }

        // Start the image movement coroutine
        movementCoroutine = StartCoroutine(MoveDisplayImage());

        // Start the red dot blinking effect coroutine
        redDotCoroutine = StartCoroutine(BlinkRedDot());
    }

    private void OnButtonClicked(ButtonInfo buttonInfo)
    {
        // Stop highlighting the previous button if one is active
        if (highlightCoroutine != null)
        {
            StopCoroutine(highlightCoroutine);
            SetButtonColor(activeButtonInfo, normalColor);
        }

        // Set the new active button and start the highlight coroutine
        activeButtonInfo = buttonInfo;
        highlightCoroutine = StartCoroutine(HighlightButton(buttonInfo.button));

        // Update the image and text based on the selected button
        displayImage.sprite = buttonInfo.image;
        displayText.text = buttonInfo.displayName;

        // Handle special behavior for Camera 6
        if (buttonInfo.buttonName == camera6ButtonName)
        {
            // Stop movement and center the display image
            if (movementCoroutine != null)
            {
                StopCoroutine(movementCoroutine);
                movementCoroutine = null;
            }
            displayImage.rectTransform.anchoredPosition = Vector3.zero; // Center the image
        }
        else
        {
            // Reset position if switching from Camera 6 to another camera view
            if (movementCoroutine == null)
            {
                displayImage.rectTransform.anchoredPosition = nonCamera6StartPosition;
                movementCoroutine = StartCoroutine(MoveDisplayImage());
            }
        }
    }

    private IEnumerator HighlightButton(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();

        if (buttonImage == null)
        {
            Debug.LogError($"Image component not found on button {button.name}");
            yield break;
        }

        float originalAlpha = buttonImage.color.a;

        // Loop to alternate colors for the highlight effect
        while (true)
        {
            buttonImage.color = new Color(highlightColor.r, highlightColor.g, highlightColor.b, originalAlpha);
            yield return new WaitForSeconds(1f);
            buttonImage.color = new Color(normalColor.r, normalColor.g, normalColor.b, originalAlpha);
            yield return new WaitForSeconds(1f);
        }
    }

    private void SetButtonColor(ButtonInfo buttonInfo, Color color)
    {
        if (buttonInfo != null)
        {
            Image buttonImage = buttonInfo.button.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = new Color(color.r, color.g, color.b, buttonImage.color.a);
            }
        }
    }

    private IEnumerator MoveDisplayImage()
    {
        Vector3 initialPosition = displayImage.rectTransform.anchoredPosition; // Store the initial position
        Vector3 targetPosition = initialPosition + Vector3.right * movementDistance; // Calculate target position

        while (true)
        {
            // Move the image towards the target position
            while (Vector3.Distance(displayImage.rectTransform.anchoredPosition, targetPosition) > 0.1f)
            {
                displayImage.rectTransform.anchoredPosition = Vector3.MoveTowards(
                    displayImage.rectTransform.anchoredPosition,
                    targetPosition,
                    movementSpeed * Time.deltaTime
                );
                yield return null;
            }

            // Pause for a moment at the target position
            yield return new WaitForSeconds(pauseDuration);

            // Reverse the direction
            Vector3 temp = initialPosition;
            initialPosition = targetPosition;
            targetPosition = temp;
        }
    }

    private IEnumerator BlinkRedDot()
    {
        while (true)
        {
            redDotImage.enabled = true; // Show the red dot
            yield return new WaitForSeconds(1.5f);
            redDotImage.enabled = false; // Hide the red dot
            yield return new WaitForSeconds(1.5f);
        }
    }
}
