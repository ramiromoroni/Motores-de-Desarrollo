using UnityEngine;

public class GateButton : MonoBehaviour, Iinteractable
{
    [SerializeField] GameObject gate; // referencia a los barrotes

    [SerializeField] AudioSource pressSfx;
    private bool isPressed = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador dentro del botón");
            other.GetComponent<InteractionManager>().SubscribeListener(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador salió del botón");
            other.GetComponent<InteractionManager>().UnsubscribeListener(this);
        }
    }

    public void Interact()
    {
        if (isPressed) // si ya se apretó, no hace nada
        {
            return;
        }

        isPressed = true;     


        Debug.Log("BOTÓN ACTIVADO — Desactivando barrotes");

        if (pressSfx != null)
        {
            pressSfx.Play();
        }

        if (gate != null)
        {
            gate.SetActive(false);
        }
    }
}
