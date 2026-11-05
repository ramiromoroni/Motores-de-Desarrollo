using System.Collections;
using UnityEngine;

public class KeycardDoorTrigger : MonoBehaviour
{
    // Variable global para saber si tenemos la tarjeta
    public static bool hasCard = false;

    [SerializeField] private Gate doorGate;         // Tu script Gate existente
    [SerializeField] private GameObject promptUI;  // El cartel UI ("Acceso con tarjeta" / "Presioná Espacio")

    private bool isPlayerNearby = false;

    private void Update()
    {
        // Si Vera está cerca y presiona Espacio
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            TryOpen();
        }
    }

    private void TryOpen()
    {
        // Solo abre si ya agarró la tarjeta previamente
        if (hasCard)
        {
            if (promptUI != null) promptUI.SetActive(false);
            if (doorGate != null) doorGate.Open(); // Ejecuta la animación y cámara de Gate
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            // Solo muestra el cartel si la puerta no está abierta todavía
            if (promptUI != null && doorGate != null)
            {
                promptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (promptUI != null) promptUI.SetActive(false);
        }
    }
}