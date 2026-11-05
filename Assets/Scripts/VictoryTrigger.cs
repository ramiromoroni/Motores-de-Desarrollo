using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField] string victorySceneName = "Victory"; // nombre de la escena de victoria

    private bool hasWon = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasWon) return; // para que no se cargue dos veces

        if (other.CompareTag("Player"))
        {
            hasWon = true;
            SceneManager.LoadScene(victorySceneName);
        }
    }
}