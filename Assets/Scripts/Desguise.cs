using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class Desguise : MonoBehaviour, Iinteractable
{
    int uses = 1;
    UnityEvent desguiseEvent;
    [SerializeField] TextMeshProUGUI interactPrompt;
    void Start()
    {
        if (desguiseEvent == null) desguiseEvent = new UnityEvent();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered Desguise trigger");
            interactPrompt.text = "[Presiona Espacio para interactuar]";
            interactPrompt.gameObject.SetActive(true);
            other.GetComponent<InteractionManager>().instance.SubscribeListener(this);
            desguiseEvent.AddListener(other.GetComponent<PlayerDesguiseManager>().SetDesguiseOn);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.gameObject.SetActive(false);
            other.GetComponent<InteractionManager>().instance.UnsubscribeListener(this);
            desguiseEvent.RemoveListener(other.GetComponent<PlayerDesguiseManager>().SetDesguiseOn);
        }
    }
    public void Interact()
    {
        Debug.Log("Player interacted with Desguise, remaining uses: " + uses);

        if (uses > 0)
        {
            desguiseEvent.Invoke();
            uses--;
            Debug.Log("Desguise used, remaining uses: " + uses);
        }
        else
        {
            interactPrompt.gameObject.SetActive(false);
            this.GetComponent<MeshRenderer>().material.color = Color.darkRed;
        }
    }
}
