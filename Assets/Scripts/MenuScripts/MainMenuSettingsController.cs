
using System.Collections;
using UnityEngine;

public class MainMenuSettingsController : MonoBehaviour
{
    [Header("Paneles principales")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Secciones de Settings")]
    public GameObject controlsPanel;
    public GameObject audioPanel;

    [Header("Animación")]
    public float animationDuration = 0.35f;

    private CanvasGroup mainMenuCanvas;
    private CanvasGroup settingsCanvas;
    private CanvasGroup controlsCanvas;
    private CanvasGroup audioCanvas;

    private bool isTransitioning = false;

    void Awake()
    {
        mainMenuCanvas = GetCanvasGroup(mainMenuPanel);
        settingsCanvas = GetCanvasGroup(settingsPanel);
        controlsCanvas = GetCanvasGroup(controlsPanel);
        audioCanvas = GetCanvasGroup(audioPanel);

        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);

        if (controlsPanel != null)
            controlsPanel.SetActive(false);

        if (audioPanel != null)
            audioPanel.SetActive(true);
    }

    private CanvasGroup GetCanvasGroup(GameObject obj)
    {
        if (obj == null)
            return null;

        CanvasGroup group = obj.GetComponent<CanvasGroup>();

        if (group == null)
            group = obj.AddComponent<CanvasGroup>();

        return group;
    }

    public void OpenSettings()
    {
        if (isTransitioning)
            return;

        StartCoroutine(ShowSettings());
    }

    private IEnumerator ShowSettings()
    {
        isTransitioning = true;

        // Ocultar el menú principal
        yield return FadeGroup(mainMenuCanvas, 0f);

        mainMenuPanel.SetActive(false);

        // Preparar Settings
        settingsPanel.SetActive(true);
        settingsCanvas.alpha = 0f;
        settingsCanvas.interactable = false;
        settingsCanvas.blocksRaycasts = false;

        if (controlsPanel != null)
            controlsPanel.SetActive(false);

        if (audioPanel != null)
            audioPanel.SetActive(true);

        // Mostrar Settings
        yield return FadeGroup(settingsCanvas, 1f);

        isTransitioning = false;
    }

    public void BackToMainMenu()
    {
        if (isTransitioning)
            return;

        StartCoroutine(ReturnToMainMenu());
    }

    private IEnumerator ReturnToMainMenu()
    {
        isTransitioning = true;

        yield return FadeGroup(settingsCanvas, 0f);

        settingsPanel.SetActive(false);

        mainMenuPanel.SetActive(true);
        mainMenuCanvas.alpha = 0f;

        yield return FadeGroup(mainMenuCanvas, 1f);

        isTransitioning = false;
    }

    public void OpenControlsSettings()
    {
        if (isTransitioning)
            return;

        StartCoroutine(ChangeSection(controlsPanel, controlsCanvas,
            audioPanel, audioCanvas));
    }

    public void OpenAudioSettings()
    {
        if (isTransitioning)
            return;

        StartCoroutine(ChangeSection(audioPanel, audioCanvas,
            controlsPanel, controlsCanvas));
    }

    private IEnumerator ChangeSection(
        GameObject panelToShow,
        CanvasGroup canvasToShow,
        GameObject panelToHide,
        CanvasGroup canvasToHide)
    {
        isTransitioning = true;

        if (panelToHide != null && panelToHide.activeSelf)
        {
            yield return FadeGroup(canvasToHide, 0f);
            panelToHide.SetActive(false);
        }

        if (panelToShow != null)
        {
            panelToShow.SetActive(true);

            if (canvasToShow != null)
            {
                canvasToShow.alpha = 0f;
                yield return FadeGroup(canvasToShow, 1f);
            }
        }

        isTransitioning = false;
    }

    private IEnumerator FadeGroup(CanvasGroup group, float targetAlpha)
    {
        if (group == null)
            yield break;

        float startAlpha = group.alpha;
        float elapsed = 0f;

        group.interactable = false;
        group.blocksRaycasts = false;

        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.SmoothStep(
                0f, 1f,
                Mathf.Clamp01(elapsed / animationDuration)
            );

            group.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            yield return null;
        }

        group.alpha = targetAlpha;
        group.interactable = targetAlpha > 0f;
        group.blocksRaycasts = targetAlpha > 0f;
    }
}
