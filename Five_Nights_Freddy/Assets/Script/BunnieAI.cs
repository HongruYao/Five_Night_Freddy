using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BunnieAI : MonoBehaviour
{
    public MonitorPanelController monitorPanelController; // Reference to the MonitorPanelController
    public LightController lightController; // Reference to the LightController
    public DoorController leftDoorController; // Reference to the left DoorController
    public GameObject failPanel; // Reference to the fail panel UI
    public Sprite cam1AReplacementSprite, cam1BReplacementSprite, cam5ReplacementSprite;
    public Sprite cam3ReplacementSprite, cam2AReplacementSprite, cam2BReplacementSprite;
    public Sprite leftLightReplacementSprite; // Replacement for leftLightOnSprite in Phase 6

    private Dictionary<string, Sprite> originalImages = new Dictionary<string, Sprite>(); // Dictionary to store original images

    void Start()
    {
        // Store the original images for each camera at the start
        foreach (var buttonInfo in monitorPanelController.buttonInfos)
        {
            originalImages[buttonInfo.buttonName] = buttonInfo.image;
        }

        // Start Phase 0, waiting for 90 seconds before beginning Bunnie's actions
        StartCoroutine(StartPhase0());
    }

    private IEnumerator StartPhase0()
    {
        yield return new WaitForSeconds(90f); // Phase 0 wait time
        StartCoroutine(Phase1());
    }

    private IEnumerator Phase1()
    {
        float waitTime = Random.Range(10f, 20f);
        yield return new WaitForSeconds(waitTime);

        // Replace images for Cam1A and Cam1B
        ReplaceCameraImage("Cam1A", cam1AReplacementSprite);
        ReplaceCameraImage("Cam1B", cam1BReplacementSprite);

        StartCoroutine(Phase2());
    }

    private IEnumerator Phase2()
    {
        float waitTime = Random.Range(23f, 33f);
        yield return new WaitForSeconds(waitTime);

        // Restore Cam1B to its original image and replace Cam5
        RestoreCameraImage("Cam1B");
        ReplaceCameraImage("Cam5", cam5ReplacementSprite);

        StartCoroutine(Phase3());
    }

    private IEnumerator Phase3()
    {
        float waitTime = Random.Range(12f, 23f);
        yield return new WaitForSeconds(waitTime);

        // Restore Cam5 to its original image and replace Cam3
        RestoreCameraImage("Cam5");
        ReplaceCameraImage("Cam3", cam3ReplacementSprite);

        StartCoroutine(Phase4());
    }

    private IEnumerator Phase4()
    {
        float waitTime = Random.Range(12f, 25f);
        yield return new WaitForSeconds(waitTime);

        // Restore Cam3 to its original image and replace Cam2A
        RestoreCameraImage("Cam3");
        ReplaceCameraImage("Cam2A", cam2AReplacementSprite);

        StartCoroutine(Phase5());
    }

    private IEnumerator Phase5()
    {
        float waitTime = Random.Range(12f, 25f);
        yield return new WaitForSeconds(waitTime);

        // Restore Cam2A to its original image and replace Cam2B
        RestoreCameraImage("Cam2A");
        ReplaceCameraImage("Cam2B", cam2BReplacementSprite);

        StartCoroutine(Phase6());
    }

    private IEnumerator Phase6()
    {
        float waitTime = Random.Range(12f, 25f);
        yield return new WaitForSeconds(waitTime);

        // Restore Cam2B to its original image and replace left light sprite in LightController
        RestoreCameraImage("Cam2B");
        lightController.leftLightOnSprite = leftLightReplacementSprite;

        StartCoroutine(Phase7());
    }

    private IEnumerator Phase7()
    {
        float waitTime = Random.Range(12f, 25f);
        yield return new WaitForSeconds(waitTime);

        // Check if the left door is open
        if (!leftDoorController.isOpen)
        {
            yield return new WaitForSeconds(2f); // Delay before showing fail panel
            failPanel.SetActive(true); // Show the fail panel if the left door is not open
        }
        else
        {
            // If the left door is open, repeat from Phase 4 to Phase 7
            StartCoroutine(Phase4());
        }
    }

    private void ReplaceCameraImage(string cameraName, Sprite replacementSprite)
    {
        foreach (var buttonInfo in monitorPanelController.buttonInfos)
        {
            if (buttonInfo.buttonName == cameraName)
            {
                buttonInfo.image = replacementSprite;
            }
        }

        // Update display if the replaced camera is currently selected
        if (monitorPanelController.activeButtonInfo != null &&
            monitorPanelController.activeButtonInfo.buttonName == cameraName)
        {
            monitorPanelController.displayImage.sprite = replacementSprite;
        }
    }

    private void RestoreCameraImage(string cameraName)
    {
        if (originalImages.TryGetValue(cameraName, out Sprite originalSprite))
        {
            foreach (var buttonInfo in monitorPanelController.buttonInfos)
            {
                if (buttonInfo.buttonName == cameraName)
                {
                    buttonInfo.image = originalSprite;
                }
            }

            // Update display if the restored camera is currently selected
            if (monitorPanelController.activeButtonInfo != null &&
                monitorPanelController.activeButtonInfo.buttonName == cameraName)
            {
                monitorPanelController.displayImage.sprite = originalSprite;
            }
        }
    }
}
