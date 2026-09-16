using UnityEngine;
using UnityEngine.Events;

public class CentralProcessor : MonoBehaviour, Iinteractable
{
    public UnityEvent processorEvent = new UnityEvent();

    private InteractionManager interactionManager;

    private void Start()
    {
        interactionManager = FindFirstObjectByType<InteractionManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionManager.SubscribeListener(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionManager.UnsubscribeListener(this);
        }
    }

    public void Interact()
    {
        processorEvent.Invoke();
    }
}