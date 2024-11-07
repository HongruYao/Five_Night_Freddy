using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailPanelController : MonoBehaviour
{
    public Button restartButton; // Reference to the restart button
    public string sceneToLoad = "Main_Level"; // Name of the scene to reload

    void Start()
    {
        // Add listener to the restart button
        restartButton.onClick.AddListener(RestartGame);
    }

    private void RestartGame()
    {
        // Reload the specified scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
