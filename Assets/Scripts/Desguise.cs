using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class Desguise : MonoBehaviour, Iinteractable
{
    int uses = 1;
    UnityEvent desguiseEvent;
    void Start()
    {
        if (desguiseEvent == null) desguiseEvent = new UnityEvent();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InteractionManager>().instance.SubscribeListener(this);
            desguiseEvent.AddListener(other.GetComponent<PlayerDesguiseManager>().SetDesguiseOn);
            Debug.Log("Player entered Desguise trigger");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InteractionManager>().instance.UnsubscribeListener(this);
            desguiseEvent.RemoveListener(other.GetComponent<PlayerDesguiseManager>().SetDesguiseOn);
            Debug.Log("Player exited Desguise trigger");
        }
    }
    public void Interact()
    {
        if (uses > 0)
        {
            desguiseEvent.Invoke();
            uses--;
            Debug.Log("Interacting with Desguise");
        }
        else
        {
            this.GetComponent<MeshRenderer>().material.color = Color.darkRed;
            Debug.Log("No more uses left for Desguise");
        }
    }
}
