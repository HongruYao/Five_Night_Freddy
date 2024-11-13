using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Button newGameButton;
    public Button continueButton;
    public CanvasGroup panelCanvasGroup;
    public float fadeDuration = 1f;
    public Image signalSignNew;
    public Image signalSignContinue;

    public SpriteRenderer tvEffectRenderer;
    public List<Sprite> tvEffectSprites;
    public float tvEffectInterval = 0.5f;

    public SpriteRenderer backgroundRenderer;
    public List<Sprite> additionalBackgroundSprites;
    public float temporaryDisplayDuration = 0.5f;

    public AudioSource backgroundMusic;
    public AudioSource soundEffect;
    public CanvasGroup secondPanelCanvasGroup;

    public Image secondPanelImage;
    public List<Sprite> secondPanelSprites;
    public TextMeshProUGUI secondPanelText;

    public string sceneToLoad; // The name of the scene to load, set in the Inspector

    void Start()
    {
        panelCanvasGroup.alpha = 0f;
        panelCanvasGroup.gameObject.SetActive(false);
        signalSignNew.gameObject.SetActive(false);
        signalSignContinue.gameObject.SetActive(false);
        secondPanelText.gameObject.SetActive(false);
        secondPanelImage.gameObject.SetActive(false);

        newGameButton.onClick.AddListener(() => StartCoroutine(ShowAndFadeOutPanel()));
        continueButton.onClick.AddListener(() => StartCoroutine(ShowAndFadeOutPanel()));

        AddHoverEvents(newGameButton, ShowSignalNew, HideSignalNew);
        AddHoverEvents(continueButton, ShowSignalContinue, HideSignalContinue);

        StartCoroutine(PlayOldTVEffect());
        StartCoroutine(BackgroundSpriteReplacementLoop());
        StartCoroutine(BackgroundColorTransitionLoop()); // Start the color transition loop
    }

    private IEnumerator ShowAndFadeOutPanel()
    {
        yield return FadeInPanel();
        yield return new WaitForSeconds(5f);

        secondPanelCanvasGroup.gameObject.SetActive(true);
        StartCoroutine(FadeInCanvasGroup(secondPanelCanvasGroup, 1f));

        yield return new WaitForSeconds(1f);
        yield return FadeOutPanel();

        backgroundMusic.Stop();
        soundEffect.Play();

        StartCoroutine(PlayImageSequence());
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator FadeInPanel()
    {
        panelCanvasGroup.gameObject.SetActive(true);
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            panelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        panelCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOutPanel()
    {
        float elapsed = 0f;
        float fadeOutDuration = 2.5f;

        while (elapsed < fadeOutDuration)
        {
            panelCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        panelCanvasGroup.alpha = 0f;
        panelCanvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator FadeInCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        canvasGroup.alpha = 0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private IEnumerator PlayImageSequence()
    {
        secondPanelImage.gameObject.SetActive(true);
        float interval = 0.4f / secondPanelSprites.Count;

        for (int i = 0; i < secondPanelSprites.Count; i++)
        {
            secondPanelImage.sprite = secondPanelSprites[i];
            yield return new WaitForSeconds(interval);
        }

        secondPanelImage.gameObject.SetActive(false);
        secondPanelText.gameObject.SetActive(true);
    }

    private IEnumerator BackgroundColorTransitionLoop()
    {
        Color originalColor = backgroundRenderer.color; // Save the original color

        while (true)
        {
            // Wait for a random interval between 2 and 3 seconds
            yield return new WaitForSeconds(Random.Range(5f, 6f));

            // Smoothly change to black
            yield return StartCoroutine(SmoothColorTransition(backgroundRenderer, Color.black, 0.5f));

            // Hold the black color for a random interval between 0.5 and 1 second
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));

            // Smoothly change back to the original color
            yield return StartCoroutine(SmoothColorTransition(backgroundRenderer, originalColor, 0.5f));
        }
    }

    private IEnumerator SmoothColorTransition(SpriteRenderer renderer, Color targetColor, float duration)
    {
        Color startColor = renderer.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            renderer.color = Color.Lerp(startColor, targetColor, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        renderer.color = targetColor;
    }

    private void AddHoverEvents(Button button, System.Action pointerEnterAction, System.Action pointerExitAction)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((eventData) => { pointerEnterAction(); });
        trigger.triggers.Add(pointerEnter);

        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((eventData) => { pointerExitAction(); });
        trigger.triggers.Add(pointerExit);
    }

    private void ShowSignalNew() => signalSignNew.gameObject.SetActive(true);
    private void HideSignalNew() => signalSignNew.gameObject.SetActive(false);
    private void ShowSignalContinue() => signalSignContinue.gameObject.SetActive(true);
    private void HideSignalContinue() => signalSignContinue.gameObject.SetActive(false);

    private IEnumerator PlayOldTVEffect()
    {
        int spriteIndex = 0;
        while (true)
        {
            tvEffectRenderer.sprite = tvEffectSprites[spriteIndex];
            spriteIndex = (spriteIndex + 1) % tvEffectSprites.Count;
            yield return new WaitForSeconds(tvEffectInterval);
        }
    }

    private IEnumerator BackgroundSpriteReplacementLoop()
    {
        Sprite originalSprite = backgroundRenderer.sprite;

        while (true)
        {
            float randomInterval = Random.Range(3f, 6f);
            yield return new WaitForSeconds(randomInterval);

            Sprite randomSprite = additionalBackgroundSprites[Random.Range(0, additionalBackgroundSprites.Count)];
            backgroundRenderer.sprite = randomSprite;

            yield return new WaitForSeconds(temporaryDisplayDuration);
            backgroundRenderer.sprite = originalSprite;

            float nextInterval = Random.Range(3f, 4f);
            yield return new WaitForSeconds(nextInterval);
        }
    }
}
