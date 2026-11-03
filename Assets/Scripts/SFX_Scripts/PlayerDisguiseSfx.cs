using UnityEngine;

// Sonidos de camuflaje (Rodry - Bloque C)
// Mira el estado del PlayerDesguiseManager, sin modificarlo:
//  suena un encastre metálico cuando Vera se pone el disfraz (con variación de pitch)
//  suena un "apagado" cuando el disfraz se termina
[RequireComponent(typeof(PlayerDesguiseManager))]
public class PlayerDisguiseSfx : MonoBehaviour
{
    [Header("Ponerse el disfraz")]
    [SerializeField] private AudioSource disguiseOnSfx;
    [SerializeField] private float onMinPitch = 0.93f;   // variación: cada vez suena apenas distinto
    [SerializeField] private float onMaxPitch = 1.07f;

    [Header("Se termina el disfraz")]
    [SerializeField] private AudioSource disguiseOffSfx;

    private PlayerDesguiseManager disguiseManager;
    private DesguiseType lastState;

    void Awake()
    {
        disguiseManager = GetComponent<PlayerDesguiseManager>();
        lastState = disguiseManager.currentDesguise;
    }

    void Update()
    {
        DesguiseType currentState = disguiseManager.currentDesguise;

        // Se lo PONE: el estado cambia a Desguise
        if (currentState == DesguiseType.Desguise && lastState != DesguiseType.Desguise)
        {
            if (disguiseOnSfx != null)
            {
                disguiseOnSfx.pitch = Random.Range(onMinPitch, onMaxPitch);
                disguiseOnSfx.Play();
            }
        }

        // Se le TERMINA: estaba con disfraz (o degradándose) y pasa a None
        bool wasDisguised = lastState == DesguiseType.Desguise || lastState == DesguiseType.Degrading;

        if (currentState == DesguiseType.None && wasDisguised)
        {
            if (disguiseOffSfx != null)
            {
                disguiseOffSfx.Play();
            }
        }

        lastState = currentState;
    }
}