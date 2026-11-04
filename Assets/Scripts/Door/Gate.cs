using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] float openHeight = 2.2f;      // cuánto sube la puerta
    [SerializeField] float openDuration = 2f;      // cuánto tarda en subir
    [SerializeField] AudioSource openSfx;          // sonido de la puerta (opcional)
    [SerializeField] GameObject doorCamera;        // cámara Cinemachine de la puerta (opcional)
    [SerializeField] float cameraBlendTime = 1f;   // espera a que la cámara llegue a la puerta
    [SerializeField] float cameraHoldTime = 0.7f;  // cuánto se queda mirando después de abrir

    private bool isOpen = false;

    public void Open()
    {
        if (isOpen) return; // si ya está abierta, no hace nada
        isOpen = true;
        StartCoroutine(OpenRoutine());
    }

    IEnumerator OpenRoutine()
    {
        // primero la cámara va hacia la puerta
        if (doorCamera != null)
        {
            doorCamera.SetActive(true);
            yield return new WaitForSeconds(cameraBlendTime);
        }

        if (openSfx != null) openSfx.Play();

        // la puerta sube de a poco
        Vector3 start = transform.localPosition;
        Vector3 end = start + Vector3.up * openHeight;
        float time = 0f;

        while (time < openDuration)
        {
            time += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(start, end, time / openDuration);
            yield return null; // espera al siguiente frame
        }
        transform.localPosition = end;

        // después la cámara vuelve a Vera
        if (doorCamera != null)
        {
            yield return new WaitForSeconds(cameraHoldTime);
            doorCamera.SetActive(false);
        }
    }
}