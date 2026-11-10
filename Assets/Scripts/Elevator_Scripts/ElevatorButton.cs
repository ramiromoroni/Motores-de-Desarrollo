using TMPro;
using UnityEngine;

public class ElevatorButton : MonoBehaviour, Iinteractable
{
    public enum ButtonAction { Use, Call } // Use = subir/bajar, Call = llamar al elevador

    [SerializeField] Elevator elevator;                  // el elevador que controla
    [SerializeField] ButtonAction action = ButtonAction.Use;
    [SerializeField] AudioSource pressSfx;               // sonido "pip"
    [SerializeField] GameObject promptUI;                // cartel del elevador
    [SerializeField] TextMeshProUGUI promptText;         // texto del cartel

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InteractionManager>().SubscribeListener(this);
            ShowPrompt();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InteractionManager>().UnsubscribeListener(this);
            promptUI.SetActive(false);
        }
    }

    public void Interact()
    {
        if (pressSfx != null) pressSfx.Play(); // suena el pip
        promptUI.SetActive(false);             // escondo el cartel durante el viaje

        if (action == ButtonAction.Use)
        {
            elevator.Use();
        }
        else
        {
            elevator.Call();
        }
    }

    // muestra el cartel con el texto según dónde está el elevador
    void ShowPrompt()
    {
        if (elevator.IsMoving) return; // si está viajando, no muestro nada

        if (action == ButtonAction.Use)
        {
            if (elevator.IsUp)
            {
                promptText.text = "Press SPACE to go down";
            }
            else
            {
                promptText.text = "Press SPACE to go up";
            }
            promptUI.SetActive(true);
        }
        else // botón de llamada
        {
            if (elevator.IsUp) // solo tiene sentido llamarlo si está arriba
            {
                promptText.text = "Press SPACE to call the elevator";
                promptUI.SetActive(true);
            }
        }
    }
}