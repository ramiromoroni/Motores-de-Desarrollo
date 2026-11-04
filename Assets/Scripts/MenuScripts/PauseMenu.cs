using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // <-- Necesario para el New Input System

public class PauseMenu : MonoBehaviour
{
    [Header("UI del Menú de Pausa")]
    public GameObject pauseMenuUI;
    public GameObject pauseTitle;
    public GameObject pauseButtons;

    [Header("Oscurecimiento")]
    public GameObject darkOverlay;

    [Header("Panel de Ajustes")]
    public GameObject settingsPanel;
    public GameObject miniMenu;

    [Header("Secciones de Ajustes")]
    public GameObject controlsPanel;
    public GameObject audioPanel;

    [Header("Configuración de Escenas")]
    public string mainMenuSceneName = "MainMenu";

    //Para el sonido de abrir el menu de pausa (zimerman)
    [Header("Sonido")]
    public AudioSource openPauseSfx;
    public AudioSource closePauseSfx;

    [Header("Animaciones")]
    public float animationDuration = 0.35f;
    public float extraSlideDistance = 50f;

    private bool isPaused = false;
    private bool isTransitioning = false;

    private RectTransform pauseRect;
    private CanvasGroup pauseCanvas;
    private CanvasGroup overlayCanvas;
    private CanvasGroup settingsCanvas;
    private CanvasGroup titleCanvas;
    private CanvasGroup buttonsCanvas;
    private CanvasGroup controlsCanvas;
    private CanvasGroup audioSectionCanvas;

    private Vector2 pauseOriginalPosition;

    void Awake()
    {
        //// Panel principal
        if (pauseMenuUI != null)
        {
            pauseRect = pauseMenuUI.GetComponent<RectTransform>();
            pauseCanvas = GetCanvasGroup(pauseMenuUI);
            pauseOriginalPosition = pauseRect.anchoredPosition;
        }

        // Fondo oscuro
        if (darkOverlay != null)
        {
            overlayCanvas = GetCanvasGroup(darkOverlay);
            overlayCanvas.alpha = 0f;
            overlayCanvas.interactable = false;
            overlayCanvas.blocksRaycasts = false;
        }

        //// Settings
        if (settingsPanel != null)
        {
            settingsCanvas = GetCanvasGroup(settingsPanel);
        }

        //// Secciones de Settings
        if (controlsPanel != null)
            controlsCanvas = GetCanvasGroup(controlsPanel);

        if (audioPanel != null)
            audioSectionCanvas = GetCanvasGroup(audioPanel);

        //// Título y botones
        if (pauseTitle != null)
            titleCanvas = GetCanvasGroup(pauseTitle);

        if (pauseButtons != null)
            buttonsCanvas = GetCanvasGroup(pauseButtons);
    }

    private CanvasGroup GetCanvasGroup(GameObject obj)
    {
        CanvasGroup group = obj.GetComponent<CanvasGroup>();

        if (group == null)
            group = obj.AddComponent<CanvasGroup>();

        return group;
    }

    void Update()
    {
        if (Keyboard.current == null || isTransitioning) return;

        if (Keyboard.current.pKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("¡Tecla detectada con New Input System!");

            if (isPaused)
            {
                if (settingsPanel != null && settingsPanel.activeSelf)
                    CloseSettings();
                else
                    Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    //// ABRIR PAUSA
    private void Pause()
    {
        if (isTransitioning)
            return;

        isPaused = true;
        Time.timeScale = 0f;

        if (openPauseSfx != null)
            openPauseSfx.Play();

        StartCoroutine(ShowPause());
    }

    private IEnumerator ShowPause()
    {
        isTransitioning = true;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (pauseTitle != null)
            pauseTitle.SetActive(true);

        if (pauseButtons != null)
            pauseButtons.SetActive(true);

        if (darkOverlay != null)
            darkOverlay.SetActive(true);

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        float distance = pauseRect.rect.width + extraSlideDistance;

        Vector2 startPosition =
            pauseOriginalPosition + Vector2.left * distance;

        pauseRect.anchoredPosition = startPosition;
        pauseCanvas.alpha = 0f;

        if (titleCanvas != null)
            titleCanvas.alpha = 1f;

        if (buttonsCanvas != null)
            buttonsCanvas.alpha = 1f;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.SmoothStep(
                0f, 1f,
                Mathf.Clamp01(elapsed / animationDuration)
            );

            pauseRect.anchoredPosition =
                Vector2.Lerp(startPosition, pauseOriginalPosition, t);

            pauseCanvas.alpha = t;

            if (overlayCanvas != null)
                overlayCanvas.alpha = 0.65f * t;

            yield return null;
        }

        pauseRect.anchoredPosition = pauseOriginalPosition;
        pauseCanvas.alpha = 1f;

        if (overlayCanvas != null)
            overlayCanvas.alpha = 0.65f;

        pauseCanvas.interactable = true;
        pauseCanvas.blocksRaycasts = true;

        isTransitioning = false;
    }

    //// CERRAR PAUSA
    public void Resume()
    {
        if (isTransitioning)
            return;

        if (closePauseSfx != null)
            closePauseSfx.Play();

        isPaused = false;
        StartCoroutine(HidePause());
    }

    private IEnumerator HidePause()
    {
        isTransitioning = true;

        //// Primero ocultar Settings si está abierto
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            yield return FadeGroup(settingsCanvas, 0f);

            settingsPanel.SetActive(false);
        }

        float distance = pauseRect.rect.width + extraSlideDistance;

        Vector2 endPosition =
            pauseOriginalPosition + Vector2.left * distance;

        float elapsed = 0f;

        pauseCanvas.interactable = false;
        pauseCanvas.blocksRaycasts = false;

        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.SmoothStep(
                0f, 1f,
                Mathf.Clamp01(elapsed / animationDuration)
            );

            pauseRect.anchoredPosition =
                Vector2.Lerp(pauseOriginalPosition, endPosition, t);

            pauseCanvas.alpha = 1f - t;

            if (overlayCanvas != null)
                overlayCanvas.alpha = 0.65f * (1f - t);

            yield return null;
        }

        pauseRect.anchoredPosition = pauseOriginalPosition;
        pauseCanvas.alpha = 1f;

        pauseMenuUI.SetActive(false);

        if (darkOverlay != null)
            darkOverlay.SetActive(false);

        Time.timeScale = 1f;
        isTransitioning = false;
    }

    //// ABRIR SETTINGS SIN MOVER EL PANEL PRINCIPAL
    public void OpenSettings()
    {
        if (isTransitioning)
            return;

        StartCoroutine(ShowSettings());
    }

    private IEnumerator ShowSettings()
    {
        isTransitioning = true;

        //// Desaparecen título y botones al mismo tiempo
        yield return FadeTwoGroups(titleCanvas, 0f, buttonsCanvas, 0f);

        if (pauseTitle != null)
            pauseTitle.SetActive(false);

        if (pauseButtons != null)
            pauseButtons.SetActive(false);

        //// Preparar Settings
        if (miniMenu != null)
            miniMenu.SetActive(true);

        if (controlsPanel != null)
            controlsPanel.SetActive(false);

        if (audioPanel != null)
            audioPanel.SetActive(false);

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            settingsCanvas.alpha = 0f;
            settingsCanvas.interactable = true;
            settingsCanvas.blocksRaycasts = true;

            yield return FadeGroup(settingsCanvas, 1f);
        }

        isTransitioning = false;
    }

    //// VOLVER AL MENÚ DE PAUSA
    public void CloseSettings()
    {
        if (isTransitioning)
            return;

        StartCoroutine(ReturnToPause());
    }

    private IEnumerator ReturnToPause()
    {
        isTransitioning = true;

        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            yield return FadeGroup(settingsCanvas, 0f);
            settingsPanel.SetActive(false);
        }

        //// Reaparecen título y botones al mismo tiempo
        if (pauseTitle != null)
        {
            pauseTitle.SetActive(true);

            if (titleCanvas != null)
                titleCanvas.alpha = 0f;
        }

        if (pauseButtons != null)
        {
            pauseButtons.SetActive(true);

            if (buttonsCanvas != null)
                buttonsCanvas.alpha = 0f;
        }

        yield return FadeTwoGroups(titleCanvas, 1f, buttonsCanvas, 1f);

        isTransitioning = false;
    }

    //// ANIMACIÓN DE TRANSPARENCIA
    private IEnumerator FadeGroup(CanvasGroup group, float targetAlpha)
    {
        if (group == null)
            yield break;

        float startAlpha = group.alpha;
        float elapsed = 0f;

        if (targetAlpha > 0f)
        {
            group.interactable = false;
            group.blocksRaycasts = false;
        }

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

    //// ANIMAR DOS ELEMENTOS SIMULTÁNEAMENTE
    private IEnumerator FadeTwoGroups(
        CanvasGroup group1,
        float targetAlpha1,
        CanvasGroup group2,
        float targetAlpha2)
    {
        if (group1 == null && group2 == null)
            yield break;

        float startAlpha1 = group1 != null ? group1.alpha : 0f;
        float startAlpha2 = group2 != null ? group2.alpha : 0f;

        float elapsed = 0f;

        if (group1 != null)
        {
            group1.interactable = false;
            group1.blocksRaycasts = false;
        }

        if (group2 != null)
        {
            group2.interactable = false;
            group2.blocksRaycasts = false;
        }

        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.SmoothStep(
                0f, 1f,
                Mathf.Clamp01(elapsed / animationDuration)
            );

            if (group1 != null)
                group1.alpha = Mathf.Lerp(startAlpha1, targetAlpha1, t);

            if (group2 != null)
                group2.alpha = Mathf.Lerp(startAlpha2, targetAlpha2, t);

            yield return null;
        }

        if (group1 != null)
        {
            group1.alpha = targetAlpha1;
            group1.interactable = targetAlpha1 > 0f;
            group1.blocksRaycasts = targetAlpha1 > 0f;
        }

        if (group2 != null)
        {
            group2.alpha = targetAlpha2;
            group2.interactable = targetAlpha2 > 0f;
            group2.blocksRaycasts = targetAlpha2 > 0f;
        }
    }

    //// SECCIONES DE SETTINGS
    public void OpenControlsSettings()
    {
        if (isTransitioning)
            return;

        StartCoroutine(ChangeSettingsSection(controlsPanel, controlsCanvas,audioPanel, audioSectionCanvas));
    }

    public void OpenAudioSettings()
    {
        if (isTransitioning)
            return;

        StartCoroutine(ChangeSettingsSection(audioPanel, audioSectionCanvas,controlsPanel, controlsCanvas));
    }


    //// CAMBIAR ENTRE SECCIONES DE SETTINGS
    private IEnumerator ChangeSettingsSection(
        GameObject panelToShow,
        CanvasGroup canvasToShow,
        GameObject panelToHide,
        CanvasGroup canvasToHide)
    {
        isTransitioning = true;

        // Desaparece la sección actual
        if (panelToHide != null && panelToHide.activeSelf)
        {
            yield return FadeGroup(canvasToHide, 0f);
            panelToHide.SetActive(false);
        }

        // Aparece la nueva sección
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

    //// REINICIAR
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //// VOLVER AL MENÚ PRINCIPAL
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        Invoke("LoadMainMenuScene", 0.2f);
    }

    private void LoadMainMenuScene()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
