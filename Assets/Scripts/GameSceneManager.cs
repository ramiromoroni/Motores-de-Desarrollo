using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("level1");
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void LoadVictory()
    {
        SceneManager.LoadScene("Victory");
    }

    public void QuitGame()
    {
        Debug.Log("Saliste del juego");
        //Application.Quit();
    }
}