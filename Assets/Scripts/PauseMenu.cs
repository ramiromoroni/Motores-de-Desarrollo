using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // <-- Necesario para el New Input System

public class PauseMenu : MonoBehaviour
{
    [Header("UI del Menú de Pausa")]
    public GameObject pauseMenuUI;

    [Header("Configuración de Escenas")]
    public string mainMenuSceneName = "MainMenu";

    //Para el sonido de abrir el menu de pausa (zimerman)
    [Header("Sonido")]                 
    public AudioSource openPauseSfx;
    public AudioSource closePauseSfx;

    private bool isPaused = false;

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.pKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("¡Tecla detectada con New Input System!");

            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        //para ejecutar el sonido al cerrar menu
        if (closePauseSfx != null)    
        {
            closePauseSfx.Play();
        }

        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Pause()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }

        //para ejecutar el sonido al abrir menu
        if (openPauseSfx != null)
        {
            openPauseSfx.Play();
        }

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

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