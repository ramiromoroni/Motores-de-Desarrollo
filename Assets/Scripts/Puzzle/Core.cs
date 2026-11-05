using UnityEngine;

public class Core : MonoBehaviour, Iinteractable
{
    [SerializeField] CorePuzzle puzzle;                 // el que cuenta los núcleos
    [SerializeField] Renderer coreRenderer;             // para cambiar el color
    [SerializeField] Color disabledColor = Color.green; // color al desactivarlo
    [SerializeField] AudioSource disableSfx;            // sonido (opcional)
    [SerializeField] ParticleSystem disableParticles;   // partículas (opcional)

    private bool isDisabled = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<InteractionManager>().SubscribeListener(this);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<InteractionManager>().UnsubscribeListener(this);
    }

    public void Interact()
    {
        if (isDisabled) return; // si ya se desactivó, no hace nada
        isDisabled = true;

        if (coreRenderer != null) coreRenderer.material.color = disabledColor;
        if (disableSfx != null) disableSfx.Play();
        if (disableParticles != null) disableParticles.Play();

        puzzle.CoreDisabled(); // le avisa al puzzle
    }
}