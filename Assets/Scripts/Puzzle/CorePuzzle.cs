using System.Collections;
using TMPro;
using UnityEngine;

public class CorePuzzle : MonoBehaviour
{
    [SerializeField] int totalCores = 3;                 // cuántos núcleos hay
    [SerializeField] TextMeshProUGUI counterText;        // texto "Cores disabled: 0/3"
    [SerializeField] DoubleDoor exitDoor;                // la puerta doble de salida
    [SerializeField] GameObject exitCamera;              // cámara Cinemachine de la salida (opcional)
    [SerializeField] float cameraBlendTime = 1f;         // espera a que llegue la cámara
    [SerializeField] float cameraHoldTime = 2f;          // cuánto se queda mirando
    [SerializeField] PlayerController playerController;  // para frenar a Vera durante la cámara

    private int disabledCores = 0;

    void Start()
    {
        UpdateCounter();
    }

    public void CoreDisabled()
    {
        disabledCores++;
        UpdateCounter();

        if (disabledCores >= totalCores) StartCoroutine(OpenExitRoutine());
    }

    void UpdateCounter()
    {
        if (counterText != null)
            counterText.text = "Cores disabled: " + disabledCores + "/" + totalCores;
    }

    // muestra la salida abriéndose y vuelve a Vera
    IEnumerator OpenExitRoutine()
    {
        if (playerController != null) playerController.enabled = false; // Vera se queda quieta

        if (exitCamera != null)
        {
            exitCamera.SetActive(true);
            yield return new WaitForSeconds(cameraBlendTime);
        }

        exitDoor.Open();

        if (exitCamera != null)
        {
            yield return new WaitForSeconds(cameraHoldTime);
            exitCamera.SetActive(false);
            yield return new WaitForSeconds(cameraBlendTime); // espera a que la cámara vuelva a Vera
        }

        if (playerController != null) playerController.enabled = true; // Vera vuelve a moverse
    }
}