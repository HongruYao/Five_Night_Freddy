using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    public TextMeshProUGUI timeText; // Reference to the TextMeshPro component for time display
    private string[] times = { "12 AM", "1 AM", "2 AM", "3 AM", "4 AM", "5 AM", "6 AM" }; // Times to display
    private int currentHourIndex = 0; // Track the current hour index
    public float timeInterval = 90f; // Time interval in seconds between each hour

    void Start()
    {
        // Initialize the time display and start the timer coroutine
        timeText.text = times[currentHourIndex];
        StartCoroutine(UpdateTime());
    }

    private IEnumerator UpdateTime()
    {
        // Loop through each time until reaching 6 AM
        while (currentHourIndex < times.Length - 1)
        {
            yield return new WaitForSeconds(timeInterval); // Wait for the specified time interval

            // Move to the next hour and update the display
            currentHourIndex++;
            timeText.text = times[currentHourIndex];
        }

        // Optionally, handle reaching 6 AM (e.g., trigger an event)
        Debug.Log("Reached 6 AM - End of the game or transition.");
    }
}
