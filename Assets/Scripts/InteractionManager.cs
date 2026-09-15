using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class InteractionManager : MonoBehaviour
{
    public InteractionManager instance;
    public UnityEvent interactionEvent;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        if (interactionEvent == null) interactionEvent = new UnityEvent();
    }
    public void SubscribeListener(Iinteractable interactable)
    {

        interactionEvent.AddListener(interactable.Interact);
    }
    public void UnsubscribeListener(Iinteractable interactable)
    {
        interactionEvent.RemoveListener(interactable.Interact);
    }
    public void OnInteraction(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            Debug.Log("Interaction input received");
            interactionEvent.Invoke();
        }
    }


}
