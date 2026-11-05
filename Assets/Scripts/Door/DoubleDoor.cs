using System.Collections;
using UnityEngine;

public class DoubleDoor : MonoBehaviour, Iinteractable
{
    [SerializeField] Transform leftDoor;                   // Door_Left
    [SerializeField] Transform rightDoor;                  // Door_Right
    [SerializeField] Vector3 slideDirection = Vector3.right; // hacia dónde va la puerta derecha
    [SerializeField] float slideDistance = 1f;             // cuánto se mete cada hoja
    [SerializeField] float openDuration = 1.5f;            // cuánto tarda en abrirse
    [SerializeField] AudioSource openSfx;                  // sonido de la puerta (opcional)
    [SerializeField] GameObject promptUI;                  // cartel "Press SPACE..." (opcional)

    private bool isOpen = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InteractionManager>().SubscribeListener(this);

            // mostramos el cartel solo si todavía está cerrada
            if (promptUI != null && !isOpen) promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InteractionManager>().UnsubscribeListener(this);

            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    public void Interact()
    {
        if (isOpen) return; // si ya está abierta, no hace nada
        isOpen = true;

        if (promptUI != null) promptUI.SetActive(false);
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