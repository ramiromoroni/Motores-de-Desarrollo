using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractionTrigger : MonoBehaviour
{
    [Header("Referencia al cartel en la UI")]
    public GameObject interactPrompt;

    [Header("Evento a Ejecutar")]
    public UnityEvent alInteractuar;

    private bool isPlayerInside = false;

    void Start()
    {
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInside)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                EjecutarInteraccion();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (interactPrompt != null)
            {
                interactPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (interactPrompt != null)
            {
                interactPrompt.SetActive(false);
            }
        }
    }

    private void EjecutarInteraccion()
    {
        //// No permite interacciones mientras el juego está pausado
        if (Time.timeScale == 0f)
            return;

        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }

        alInteractuar?.Invoke();
    }
}