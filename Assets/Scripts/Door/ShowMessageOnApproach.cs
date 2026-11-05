using UnityEngine;

public class ShowMessageOnApproach : MonoBehaviour
{
    [SerializeField] private GameObject messageUI; // El cartel "Puerta trabada"

    private void OnTriggerEnter(Collider other)
    {
        // Detecta si es Vera / el jugador
        if (other.CompareTag("Player"))
        {
            if (messageUI != null) messageUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Lo oculta al alejarse
        if (other.CompareTag("Player"))
        {
            if (messageUI != null) messageUI.SetActive(false);
        }
    }
}