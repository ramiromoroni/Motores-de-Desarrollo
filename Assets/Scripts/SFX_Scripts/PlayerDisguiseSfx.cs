using UnityEngine;

// Sonido de camuflaje (Rodry - Bloque C)
// Mira el estado del PlayerDesguiseManager y suena cuando Vera se pone el disfraz,
// sin modificar el script de mi compañero.
[RequireComponent(typeof(PlayerDesguiseManager))]
public class PlayerDisguiseSfx : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource disguiseOnSfx;

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

        // Suena solo en el momento en que el estado CAMBIA a Desguise
        if (currentState == DesguiseType.Desguise && lastState != DesguiseType.Desguise)
        {
            if (disguiseOnSfx != null)
            {
                disguiseOnSfx.Play();
            }
        }

        lastState = currentState;
    }
}