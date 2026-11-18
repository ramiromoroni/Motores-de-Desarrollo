using System.Collections;
using TMPro;
using UnityEngine;

public class DoubleDoor : MonoBehaviour, Iinteractable
{
    [SerializeField] Transform leftDoor;                     // Door_Left
    [SerializeField] Transform rightDoor;                    // Door_Right
    [SerializeField] Vector3 slideDirection = Vector3.right; // hacia dónde va la puerta derecha
    [SerializeField] float slideDistance = 1f;               // cuánto se mete cada hoja
    [SerializeField] float openDuration = 1.5f;              // cuánto tarda en abrirse
    [SerializeField] AudioSource openSfx;                    // sonido de la puerta (opcional)
    [SerializeField] TextMeshProUGUI promptUI;                    // cartel "Press SPACE..." (opcional)
    [SerializeField] bool openWithInteraction = true;        // false = solo la abre el puzzle

    private bool isOpen = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // mostramos el cartel solo si todavía está cerrada
            if (promptUI != null && !isOpen)
            {
                promptUI.text = "[Esta puerta podría abrirse con la energía necesaria...]";
                promptUI.gameObject.SetActive(true);
            }
            if (!openWithInteraction) return; // si es la salida del puzzle, Space no hace nada
            other.GetComponent<InteractionManager>().SubscribeListener(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (promptUI != null) promptUI.gameObject.SetActive(false);
        if (!openWithInteraction) return;

        if (other.CompareTag("Player"))
        {
            other.GetComponent<InteractionManager>().UnsubscribeListener(this);
        }
    }

    // se abre cuando Vera aprieta Space
    public void Interact()
    {
        if (promptUI != null) promptUI.gameObject.SetActive(false);
        Open();
    }

    // se abre desde otro script (por ejemplo, el puzzle)
    public void Open()
    {
        if (isOpen) return; // si ya está abierta, no hace nada
        isOpen = true;

        if (openSfx != null) openSfx.Play();
        StartCoroutine(OpenRoutine());
    }

    // las dos hojas se deslizan a la vez, cada una para su lado
    IEnumerator OpenRoutine()
    {
        Vector3 offset = slideDirection.normalized * slideDistance;

        Vector3 leftStart = leftDoor.localPosition;
        Vector3 rightStart = rightDoor.localPosition;
        Vector3 leftEnd = leftStart - offset;   // la izquierda va para el lado contrario
        Vector3 rightEnd = rightStart + offset;

        float time = 0f;

        while (time < openDuration)
        {
            time += Time.deltaTime;
            float t = time / openDuration;
            leftDoor.localPosition = Vector3.Lerp(leftStart, leftEnd, t);
            rightDoor.localPosition = Vector3.Lerp(rightStart, rightEnd, t);
            yield return null;
        }

        leftDoor.localPosition = leftEnd;
        rightDoor.localPosition = rightEnd;
    }
}