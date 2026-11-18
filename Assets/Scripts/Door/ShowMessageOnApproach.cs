using TMPro;
using UnityEngine;

public class ShowMessageOnApproach : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageUI; // El cartel "Puerta trabada"

    private void OnTriggerEnter(Collider other)
    {
        // Detecta si es Vera / el jugador
        if (other.CompareTag("Player"))
        {
            if (messageUI != null)
            {
                messageUI.text = "Tal vez con una tarjeta de acceso pueda abrirse...";
                messageUI.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Lo oculta al alejarse
        if (other.CompareTag("Player"))
        {
            if (messageUI != null) messageUI.gameObject.SetActive(false);
        }
    }
}