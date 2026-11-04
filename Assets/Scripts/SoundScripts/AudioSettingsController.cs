using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class AudioSettingsController : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Slider de Música")]
    public Slider musicSlider;
    public TMP_Text musicPercentage;

    [Header("Slider de Efectos")]
    public Slider sfxSlider;
    public TMP_Text sfxPercentage;

    private void Start()
    {
        //// Configuración inicial de los sliders
        musicSlider.minValue = 0f;
        musicSlider.maxValue = 1f;

        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
    }

    public void SetMusicVolume(float value)
    {
        Debug.Log("MUSIC SLIDER RECIBE: " + value);

        SetMixerVolume("MusicVolume", value);

        if (musicPercentage != null)
        {
            musicPercentage.text = Mathf.RoundToInt(value * 100f) + "%";
        }
    }

    public void SetSFXVolume(float value)
    {
        Debug.Log("SFX SLIDER RECIBE: " + value);

        SetMixerVolume("SFXVolume", value);

        if (sfxPercentage != null)
        {
            sfxPercentage.text = Mathf.RoundToInt(value * 100f) + "%";
        }
    }

    private void SetMixerVolume(string parameter, float value)
    {
        if (audioMixer == null)
        {
            Debug.LogError("Audio Mixer no está asignado.");
            return;
        }

        float volumeDB = value <= 0.0001f
            ? -80f
            : Mathf.Log10(value) * 20f;

        bool applied = audioMixer.SetFloat(parameter, volumeDB);

        Debug.Log(
            parameter + " | Slider: " + value +
            " | Volumen dB: " + volumeDB +
            " | Aplicado: " + applied
        );
    }
}
////Peleando como boluda con el audiomixer y resulta que habia asignado algo mal yo