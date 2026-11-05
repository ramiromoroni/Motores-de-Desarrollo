using UnityEngine;
using UnityEngine.Audio;

public class AudioMixManager : MonoBehaviour
{
    public static AudioMixManager Instance { get; private set; }

    public enum AudioChannel
    {
        Master,
        Ambience,
        Music,
        SFX,
        Voice
    }

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Snapshots")]
    [SerializeField] private AudioMixerSnapshot area1;
    [SerializeField] private AudioMixerSnapshot area2;
    [Min(0f)] public float transicion = 1f;

    [Header("Initial Volumes")]
    [Range(0f, 1f)][SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float ambienceVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float musicVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float sfxVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float voiceVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetVolume(AudioChannel.Master, masterVolume);
        SetVolume(AudioChannel.Ambience, ambienceVolume);
        SetVolume(AudioChannel.Music, musicVolume);
        SetVolume(AudioChannel.SFX, sfxVolume);
        SetVolume(AudioChannel.Voice, voiceVolume);

        Area1();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void SetVolume(AudioChannel channel, float volume)
    {
        if (audioMixer == null)
        {
            Debug.LogWarning("AudioMixer no asignado.");
            return;
        }

        string parameter = channel switch
        {
            AudioChannel.Master => "MasterVolume",
            AudioChannel.Ambience => "AmbienceVolume",
            AudioChannel.Music => "MusicVolume",
            AudioChannel.SFX => "SFXVolume",
            AudioChannel.Voice => "VoiceVolume",
            _ => null
        };

        if (parameter == null)
            return;

        volume = Mathf.Clamp01(volume);

        float decibels = volume > 0f
            ? Mathf.Log10(volume) * 20f
            : -80f;

        audioMixer.SetFloat(parameter, decibels);
    }

    public void Area1()
    {
        area1?.TransitionTo(transicion);
    }

    public void Area2()
    {
        area2?.TransitionTo(transicion);
    }
}