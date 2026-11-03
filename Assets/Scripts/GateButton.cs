using System.Collections;
using UnityEngine;

public class GateButton : MonoBehaviour, Iinteractable
{
    [SerializeField] GameObject gate; // referencia a los barrotes
    [SerializeField] AudioSource pressSfx;
    [SerializeField] Transform buttonTop;       // el cilindro rojo del centro
    [SerializeField] float pressDepth = 0.05f;  // cuánto baja
    [SerializeField] float pressDuration = 0.2f; // cuánto tarda en bajar

    [SerializeField] Renderer buttonTopRenderer;          // el renderer del cilindro rojo
    [SerializeField] Color pressedColor = Color.green;    // color al apretarlo

    private bool isPressed = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InteractionManager>().SubscribeListener(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InteractionManager>().UnsubscribeListener(this);
        }
    }

    public void Interact()
    {
        if (isPressed) return; // si ya se apretó, no hace nada
        isPressed = true;

        if (pressSfx != null) pressSfx.Play();

        StartCoroutine(PressRoutine());

        if (gate != null) gate.SetActive(false); // por ahora sigue igual

        if (buttonTopRenderer != null) buttonTopRenderer.material.color = pressedColor;
    }

    // el cilindro rojo baja suave y se queda abajo
    IEnumerator PressRoutine()
    {
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