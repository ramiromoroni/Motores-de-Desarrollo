using UnityEngine;

// Pasos del robot (Rodry - Bloque C)
// Lee el Animator del robot ("Walking" / "Running") para saber si patrulla o persigue,
// sin modificar el EnemyController.
public class EnemyFootsteps : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource stepsSource;
    [SerializeField] private Animator enemyAnimator;

    [Header("Intervalo entre pasos (segundos)")]
    [SerializeField] private float patrolStepInterval = 1.0f;  // patrullando: lento y pesado
    [SerializeField] private float chaseStepInterval = 0.5f;   // persiguiendo: más rápido

    [Header("Pitch (variación aleatoria)")]
    [SerializeField] private float minPitch = 0.62f;           // grave = pesado
    [SerializeField] private float maxPitch = 0.72f;

    [Header("Volumen")]
    [SerializeField] private float patrolVolume = 0.8f;
    [SerializeField] private float chaseVolume = 1.0f;         // persiguiendo: más fuerte

    private float stepTimer = 0f;

    void Awake()
    {
        if (enemyAnimator == null)
        {
            enemyAnimator = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        if (enemyAnimator == null)
        {
            return;
        }

        bool isRunning = enemyAnimator.GetBool("Running");
        bool isWalking = enemyAnimator.GetBool("Walking");

        if (isRunning)
        {
            HandleFootsteps(chaseStepInterval, chaseVolume);
        }
        else if (isWalking)
        {
            HandleFootsteps(patrolStepInterval, patrolVolume);
        }
        else
        {
            stepTimer = 0f;
        }
    }

    private void HandleFootsteps(float interval, float volume)
    {
        if (stepsSource == null)
        {
            return;
        }

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            stepsSource.pitch = Random.Range(minPitch, maxPitch);
            stepsSource.PlayOneShot(stepsSource.clip, volume);
            stepTimer = interval;
        }
    }
}
