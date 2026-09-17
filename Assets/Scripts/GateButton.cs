using UnityEngine;

public class GateButton : MonoBehaviour, Iinteractable
{
    [SerializeField] GameObject gate; // referencio a los barrotes

    //Verifico colision con el jugador
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InteractionManager>().instance.SubscribeListener(this);
        }
    }

    //verifico si ya salio de mi colision 
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InteractionManager>().instance.UnsubscribeListener(this);
        }
    }


    public void Interact()
    {
        Debug.Log("BOTÓN ACTIVADO — Desactivando barrotes");
        if (gate != null)
        {
            gate.SetActive(false); // desactiva los barrotes
        }
    }
}

