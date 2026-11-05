using UnityEngine;

public class Core : MonoBehaviour, Iinteractable
{
    [SerializeField] CorePuzzle puzzle;               // el que cuenta los núcleos
    [SerializeField] GameObject activeModel;          // celda roja
    [SerializeField] GameObject disabledModel;        // celda verde
    [SerializeField] AudioSource disableSfx;          // sonido (opcional)
    [SerializeField] ParticleSystem disableParticles; // partículas (opcional)
    [SerializeField] AudioSource idleLoop;            //el sonido que va en loop

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

        if (activeModel != null) activeModel.SetActive(false);   // escondemos la roja
        if (disabledModel != null) disabledModel.SetActive(true); // mostramos la verde
        if (disableSfx != null) disableSfx.Play();
        if (disableParticles != null) disableParticles.Play();
        if (idleLoop != null) idleLoop.Stop();                    // detiene el loop

        puzzle.CoreDisabled(); // le avisa al puzzle
    }

  
}