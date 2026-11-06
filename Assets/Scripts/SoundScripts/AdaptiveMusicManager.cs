
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveMusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource explorationSource;
    public AudioSource detectionSource;
    public AudioSource pursuitSource;

    [Header("Music Clips")]
    public AudioClip explorationClip;
    public AudioClip detectionClip;
    public AudioClip pursuitClip;

    [Header("Crossfade")]
    public float crossfadeDuration = 1.5f;

    [Header("Detection Memory")]
    public float detectionMemoryDuration = 5f;

    private Coroutine detectionTimer;
    private bool detectionMemoryActive = false;


    private Coroutine fadeCoroutine;
    private AudioSource currentTarget;

    private HashSet<SecurityCamera> detectedCameras = new HashSet<SecurityCamera>();
    private HashSet<EnemyController> pursuingEnemies = new HashSet<EnemyController>();

    private void Start()
    {
        if (explorationSource == null || detectionSource == null || pursuitSource == null ||
            explorationClip == null || detectionClip == null || pursuitClip == null)
        {
            Debug.LogError("AdaptiveMusicManager: faltan Audio Sources o clips.");
            enabled = false;
            return;
        }

        explorationSource.clip = explorationClip;
        detectionSource.clip = detectionClip;
        pursuitSource.clip = pursuitClip;

        explorationSource.loop = true;
        detectionSource.loop = true;
        pursuitSource.loop = true;

        explorationSource.playOnAwake = false;
        detectionSource.playOnAwake = false;
        pursuitSource.playOnAwake = false;

        explorationSource.volume = 0.5f;
        detectionSource.volume = 0f;
        pursuitSource.volume = 0f;

        currentTarget = explorationSource;

        double startTime = AudioSettings.dspTime + 0.1;

        explorationSource.PlayScheduled(startTime);
        detectionSource.PlayScheduled(startTime);
        pursuitSource.PlayScheduled(startTime);
    }

    public void RegisterCameraDetection(SecurityCamera camera)
    {
        if (camera == null) return;

        detectedCameras.Add(camera);

        if (detectionTimer != null)
        {
            StopCoroutine(detectionTimer);
            detectionTimer = null;
        }

        detectionMemoryActive = true;
        UpdateMusicState();
    }

    public void UnregisterCameraDetection(SecurityCamera camera)
    {
        if (camera == null) return;

        bool wasRegistered = detectedCameras.Remove(camera);

        if (!wasRegistered) return;

        if (detectedCameras.Count == 0)
        {
            if (detectionTimer != null)
            {
                StopCoroutine(detectionTimer);
            }

            detectionTimer = StartCoroutine(DetectionMemoryTimer());
        }

        UpdateMusicState();
    }

    private IEnumerator DetectionMemoryTimer()
    {
        yield return new WaitForSeconds(detectionMemoryDuration);

        detectionMemoryActive = false;
        detectionTimer = null;

        UpdateMusicState();
    }

    public void BeginPursuit(EnemyController enemy)
    {
        if (enemy == null) return;

        pursuingEnemies.Add(enemy);
        UpdateMusicState();
    }

    public void EndPursuit(EnemyController enemy)
    {
        if (enemy == null) return;

        pursuingEnemies.Remove(enemy);
        UpdateMusicState();
    }

    private void UpdateMusicState()
    {
        if (pursuingEnemies.Count > 0)
        {
            ChangeMusic(pursuitSource);
        }
        else if (detectedCameras.Count > 0 || detectionMemoryActive)
        {
            ChangeMusic(detectionSource);
        }
        else
        {
            ChangeMusic(explorationSource);
        }
    }

    private void ChangeMusic(AudioSource target)
    {
        if (currentTarget == target) return;

        currentTarget = target;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(Crossfade(target));
    }


    private IEnumerator Crossfade(AudioSource target)
    {
        float startExploration = explorationSource.volume;
        float startDetection = detectionSource.volume;
        float startPursuit = pursuitSource.volume;

        float elapsed = 0f;

        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = crossfadeDuration > 0f
                ? Mathf.Clamp01(elapsed / crossfadeDuration)
                : 1f;

            explorationSource.volume = Mathf.Lerp(
                startExploration,
                target == explorationSource ? 0.5f : 0f,
                t
            );

            detectionSource.volume = Mathf.Lerp(
                startDetection,
                target == detectionSource ? 0.5f : 0f,
                t
            );

            pursuitSource.volume = Mathf.Lerp(
                startPursuit,
                target == pursuitSource ? 0.5f : 0f,
                t
            );

            yield return null;
        }

        explorationSource.volume = target == explorationSource ? 0.5f : 0f;
        detectionSource.volume = target == detectionSource ? 0.5f : 0f;
        pursuitSource.volume = target == pursuitSource ? 0.5f : 0f;

        fadeCoroutine = null;
    }


}
