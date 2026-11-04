
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [Header("Elementos de la interfaz")]
    public GameObject gameOverCanvas;
    public RectTransform captureImage;
    public CanvasGroup captureCanvas;
    public CanvasGroup lightCanvas;
    public CanvasGroup menuCanvas;

    [Header("Configuración de la secuencia")]
    public float zoomDuration = 3f;
    public float lightDuration = 2f;
    public float menuFadeDuration = 1f;

    [Header("Zoom de la imagen")]
    public float startingScale = 1f;
    public float finalScale = 1.25f;

    [Header("Intensidad de la luz")]
    [Range(0f, 1f)]
    public float finalLightAlpha = 0.85f;

    [Header("Otros sistemas")]
    public PauseMenu pauseMenu;

    private bool gameOverStarted = false;
    private Vector3 originalScale;

    void Awake()
    {
        if (captureImage != null)
            originalScale = captureImage.localScale;

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);
    }

    public void StartGameOver()
    {
        if (gameOverStarted)
            return;

        gameOverStarted = true;

        Time.timeScale = 0f;

        if (pauseMenu != null)
            pauseMenu.BlockPauseForGameOver();

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        captureCanvas.alpha = 1f;
        captureImage.localScale = originalScale * startingScale;

        lightCanvas.alpha = 0f;
        menuCanvas.alpha = 0f;

        menuCanvas.interactable = false;
        menuCanvas.blocksRaycasts = false;

        float elapsed = 0f;

        // Primera etapa: zoom de Vera
        while (elapsed < zoomDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.SmoothStep(
                0f, 1f,
                Mathf.Clamp01(elapsed / zoomDuration)
            );

            captureImage.localScale = Vector3.Lerp(
                originalScale * startingScale,
                originalScale * finalScale,
                t
            );

            yield return null;
        }

        // Segunda etapa: la luz se intensifica
        elapsed = 0f;

        while (elapsed < lightDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.SmoothStep(
                0f, 1f,
                Mathf.Clamp01(elapsed / lightDuration)
            );

            lightCanvas.alpha = Mathf.Lerp(0f, finalLightAlpha, t);

            yield return null;
        }

        // Tercera etapa: aparece el menú
        elapsed = 0f;

        while (elapsed < menuFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.SmoothStep(
                0f, 1f,
                Mathf.Clamp01(elapsed / menuFadeDuration)
            );

            menuCanvas.alpha = t;

            yield return null;
        }

        menuCanvas.alpha = 1f;
        menuCanvas.interactable = true;
        menuCanvas.blocksRaycasts = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
