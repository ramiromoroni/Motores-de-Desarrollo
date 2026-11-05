using UnityEngine;

public class KeycardItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Detecta si Vera pisa la tarjeta
        if (other.CompareTag("Player"))
        {
            // Le avisa al sistema que ya tenemos la tarjeta
            KeycardDoorTrigger.hasCard = true;

            // Opcional: podrías reproducir un SFX de pickup acá

            // Desaparece la tarjeta del mapa
            gameObject.SetActive(false);
        }
    }
}