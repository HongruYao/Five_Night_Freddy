using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerController : MonoBehaviour
{
    public float initialPower = 99f; // Starting power percentage
    private float currentPower;
    public TextMeshProUGUI powerText; // Reference to TextMeshPro for power display
    public Image usageImage; // Reference to Image component for usage degree display
    public List<Sprite> usageSprites; // List of sprites for each usage degree

    private int usageDegree = 1; // Default usage degree
    private float[] drainRates = { 8f, 4f, 3f, 2f }; // Drain rates per degree (in seconds for each 1% power)
    private float drainTimer; // Timer for power drain

    private int activeSystems = 0; // Track the number of active systems (Light/Door/Monitor)

    void Start()
    {
        currentPower = initialPower;
        UpdatePowerText();
        UpdateUsageDisplay();
    }

    void Update()
    {
        // Drain power based on usage degree
        drainTimer += Time.deltaTime;
        float drainDuration = drainRates[usageDegree - 1];

        if (drainTimer >= drainDuration)
        {
            drainTimer = 0f;
            DecreasePower(1);
        }
    }

    public void ToggleSystem(bool isActive)
    {
        activeSystems += isActive ? 1 : -1;
        activeSystems = Mathf.Clamp(activeSystems, 0, 3);

        // Determine the new usage degree based on active systems
        usageDegree = Mathf.Clamp(activeSystems + 1, 1, 4);
        UpdateUsageDisplay();
    }

    private void DecreasePower(int amount)
    {
        currentPower -= amount;
        if (currentPower < 0) currentPower = 0;

        UpdatePowerText();

        if (currentPower <= 0)
        {
            // Handle game over due to power loss
            Debug.Log("Game Over - Out of Power!");
        }
    }

    private void UpdatePowerText()
    {
        powerText.text = "Power Left: " + Mathf.RoundToInt(currentPower) + "%";
    }

    private void UpdateUsageDisplay()
    {
        // Update usage image based on current usage degree
        usageImage.sprite = usageSprites[usageDegree - 1];
    }
}
