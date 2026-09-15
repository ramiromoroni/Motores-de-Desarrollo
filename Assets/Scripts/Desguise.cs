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
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InteractionManager>().instance.UnsubscribeListener(this);
            desguiseEvent.RemoveListener(other.GetComponent<PlayerDesguiseManager>().SetDesguiseOn);
        }
    }
    public void Interact()
    {
        if (uses > 0)
        {
            desguiseEvent.Invoke();
            uses--;
        }
        else
        {
            this.GetComponent<MeshRenderer>().material.color = Color.darkRed;
        }
    }
}
