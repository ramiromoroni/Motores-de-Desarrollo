using UnityEngine;

public class GateButton : MonoBehaviour, Iinteractable
{
    [SerializeField] GameObject gate; // referencia a los barrotes

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
        Debug.Log("BOTÓN ACTIVADO — Desactivando barrotes");
        if (gate != null)
        {
            gate.SetActive(false);
        }
    }
}
