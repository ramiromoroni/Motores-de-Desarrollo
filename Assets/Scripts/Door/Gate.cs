using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] float openHeight = 2.2f;    // cuánto sube la reja
    [SerializeField] float openDuration = 2f;  // cuánto tarda en subir
    [SerializeField] AudioSource openSfx;      // sonido de la puerta (opcional)

    private bool isOpen = false;

    public void Open()
    {
        if (isOpen) return; // si ya está abierta, no hace nada
        isOpen = true;
        StartCoroutine(OpenRoutine());
    }

    // la puerta sube de a poco y queda arriba
    IEnumerator OpenRoutine()
    {
        if (openSfx != null) openSfx.Play();

        Vector3 start = transform.localPosition;
        Vector3 end = start + Vector3.up * openHeight;
        float time = 0f;

        while (time < openDuration)
        {
            time += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(start, end, time / openDuration);
            yield return null;
        }
        transform.localPosition = end;
    }
}