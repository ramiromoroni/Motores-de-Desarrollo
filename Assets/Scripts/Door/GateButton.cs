using System.Collections;
using UnityEngine;

public class GateButton : MonoBehaviour, Iinteractable
{
    [SerializeField] Gate gate;                          // referencia a la puerta (Door)
    [SerializeField] AudioSource pressSfx;
    [SerializeField] Transform buttonTop;                // el cilindro rojo del centro
    [SerializeField] Renderer buttonTopRenderer;         // el renderer del cilindro rojo
    [SerializeField] Color pressedColor = Color.green;   // color al apretarlo
    [SerializeField] float pressDepth = 0.05f;           // cuánto baja
    [SerializeField] float pressDuration = 0.2f;         // cuánto tarda en bajar

    [SerializeField] ParticleSystem pressParticles; // estrellitas al apretar

    [SerializeField] GameObject promptUI; // cartel "Press SPACE to open the door"
    private bool isPressed = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InteractionManager>().SubscribeListener(this);

            //muestro el cartel SOLO si todavía no se apretó el boton
            if (promptUI != null && !isPressed) promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InteractionManager>().UnsubscribeListener(this);

            // dejo de mostrar el cartel al alejarse
            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    public void Interact()
    {
        if (isPressed) return; // si ya se apretó, no hace nada
        isPressed = true;

        if (pressSfx != null) pressSfx.Play();
        if (pressParticles != null) pressParticles.Play();
        if (buttonTopRenderer != null) buttonTopRenderer.material.color = pressedColor;

        StartCoroutine(PressRoutine());

        if (gate != null) gate.Open();
    }

    // el cilindro rojo baja suave y se queda abajo
    IEnumerator PressRoutine()
    {
        if (buttonTop == null) yield break; // si no hay cilindro asignado, no hace nada

        Vector3 start = buttonTop.localPosition;
        Vector3 end = start + Vector3.down * pressDepth;
        float time = 0f;

        while (time < pressDuration)
        {
            time += Time.deltaTime;
            buttonTop.localPosition = Vector3.Lerp(start, end, time / pressDuration);
            yield return null;
        }

        buttonTop.localPosition = end;
    }
}