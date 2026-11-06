
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
        Debug.Log("AudioSettingsController: START ejecutado");
        //// Configurar slider de música
        if (musicSlider != null)
        {
            musicSlider.minValue = 0f;
            musicSlider.maxValue = 1f;
            ////Alguien que tenga mas fuerza de voluntad que yo y que arregle la continuidad del volumen
            ////No tengo idea de que es lo que resetea el volumen del menu de pausa a 1
            float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 1f);
            Debug.Log("LEVEL1 - Music guardado: " + PlayerPrefs.GetFloat("MusicVolume", -1f));

            musicSlider.SetValueWithoutNotify(savedMusic);
            SetMusicVolume(savedMusic);
            Debug.Log("CARGANDO AUDIO - Música: " + savedMusic);


        }

        //// Configurar slider de efectos
        if (sfxSlider != null)
        {
            sfxSlider.minValue = 0f;
            sfxSlider.maxValue = 1f;

            float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 1f);
            Debug.Log("LEVEL1 - SFX guardado: " + PlayerPrefs.GetFloat("SFXVolume", -1f));

            sfxSlider.SetValueWithoutNotify(savedSFX);
            SetSFXVolume(savedSFX);
            Debug.Log("CARGANDO AUDIO - Efectos: " + savedSFX);
        }

    }

    public void SetMusicVolume(float value)
    {
        Debug.Log("SetMusicVolume recibió: " + value);
        value = Mathf.Clamp01(value);

        SetMixerVolume("MusicVolume", value);

        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        Debug.Log("MENÚ PRINCIPAL GUARDA MÚSICA: " + PlayerPrefs.GetFloat("MusicVolume"));

        if (musicPercentage != null)
        {
            musicPercentage.text = Mathf.RoundToInt(value * 100f) + "%";
        }
    }

    public void SetSFXVolume(float value)
    {
        Debug.Log("SetSFXVolume recibió: " + value);
        value = Mathf.Clamp01(value);

        SetMixerVolume("SFXVolume", value);

        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
        Debug.Log("MENÚ PRINCIPAL GUARDA EFECTOS: " + PlayerPrefs.GetFloat("SFXVolume"));

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