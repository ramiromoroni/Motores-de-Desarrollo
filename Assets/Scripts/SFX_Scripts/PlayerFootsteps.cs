using UnityEngine;

// Pasos de Vera (Rodry - Bloque C)
// Lee el estado del PlayerController (correr / agacharse) sin modificarlo.
[RequireComponent(typeof(PlayerController))]
public class PlayerFootsteps : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource stepsSource;
    [SerializeField] private AudioLowPassFilter stepsFilter;

    [Header("Intervalo entre pasos (segundos)")]
    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float runStepInterval = 0.25f;
    [SerializeField] private float crouchStepInterval = 0.8f;

    [Header("Pitch (variación aleatoria)")]
    [SerializeField] private float walkMinPitch = 1.08f;
    [SerializeField] private float walkMaxPitch = 1.18f;
    [SerializeField] private float runMinPitch = 1.15f;
    [SerializeField] private float runMaxPitch = 1.28f;
    [SerializeField] private float crouchMinPitch = 0.92f;
    [SerializeField] private float crouchMaxPitch = 0.98f;

    [Header("Volumen")]
    [SerializeField] private float walkVolume = 0.7f;
    [SerializeField] private float runVolume = 1.0f;
    [SerializeField] private float crouchVolume = 0.2f;

    [Header("Filtro al agacharse")]
    [SerializeField] private float crouchCutoff = 1500f;

    [Header("Detección de movimiento")]
    [SerializeField] private float minMoveSpeed = 0.5f; // por debajo de esto se considera quieta

    private PlayerController player;
    private Vector3 lastPosition;
    private float stepTimer = 0f;

    void Awake()
    {
        player = GetComponent<PlayerController>();
        lastPosition = transform.position;
    }

    void Update()
    {
        // Con el juego en pausa (timeScale = 0) no hacemos nada
        if (Time.deltaTime <= 0f)
        {
            return;
        }

        // Velocidad horizontal real de Vera (ignora la gravedad)
        Vector3 movement = transform.position - lastPosition;
        movement.y = 0f;
        float speed = movement.magnitude / Time.deltaTime;
        lastPosition = transform.position;

        if (speed > minMoveSpeed)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayStep();
                stepTimer = GetStepInterval();
            }
        }
        else
        {
            // Quieta: el próximo paso suena apenas vuelva a moverse
            stepTimer = 0f;
        }
    }

    private float GetStepInterval()
    {
        if (player.isCrouching)
        {
            return crouchStepInterval;
        }
        else if (player.isRunning)
        {
            return runStepInterval;
        }
        else
        {
            return walkStepInterval;
        }
    }

    private void PlayStep()
    {
        if (stepsSource == null)
        {
            return;
        }

        float volume;

        if (player.isCrouching)
        {
            stepsSource.pitch = Random.Range(crouchMinPitch, crouchMaxPitch);
            volume = crouchVolume;

            if (stepsFilter != null)
            {
                stepsFilter.cutoffFrequency = crouchCutoff; // suena amortiguado
            }
        }
        else if (player.isRunning)
        {
            stepsSource.pitch = Random.Range(runMinPitch, runMaxPitch);
            volume = runVolume;

            if (stepsFilter != null)
            {
                stepsFilter.cutoffFrequency = 22000f; // sin filtro
            }
        }
        else
        {
            stepsSource.pitch = Random.Range(walkMinPitch, walkMaxPitch);
            volume = walkVolume;

            if (stepsFilter != null)
            {
                stepsFilter.cutoffFrequency = 22000f; // sin filtro
            }
        }

        stepsSource.PlayOneShot(stepsSource.clip, volume);
    }
}
