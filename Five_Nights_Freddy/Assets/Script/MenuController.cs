using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button startButton; // Reference to the start button
    public GameObject startPanel; // Reference to the panel that appears after clicking start
    public string mainLevelScene = "Main_Level"; // Name of the main level scene to load

    void Start()
    {
        // Ensure the start panel is initially hidden
        startPanel.SetActive(false);

        // Add listener to the start button
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        // Show the start panel
        startPanel.SetActive(true);

        // Start the coroutine to load the main level after 5 seconds
        StartCoroutine(LoadMainLevelAfterDelay());
    }

    private IEnumerator LoadMainLevelAfterDelay()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Load the main level scene
        SceneManager.LoadScene(mainLevelScene);
    }
}
