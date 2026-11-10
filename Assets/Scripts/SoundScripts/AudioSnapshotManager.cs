using UnityEngine;
using UnityEngine.Audio;

public class AudioSnapshotManager : MonoBehaviour
{
    [Header("Audio Snapshots")]
    [SerializeField] private AudioMixerSnapshot snapshot1;
    [SerializeField] private AudioMixerSnapshot snapshot2;

    [Header("Transition")]
    [Min(0f)]
    [SerializeField] private float transitionTime = 1f;

    [Header("Player")]
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
            TransitionTo(snapshot2);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
            TransitionTo(snapshot1);
    }

    private void TransitionTo(AudioMixerSnapshot snapshot)
    {
        snapshot?.TransitionTo(transitionTime);
    }
}
