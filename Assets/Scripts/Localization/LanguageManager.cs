
using UnityEngine;
using System;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    public enum Language
    {
        Spanish,
        English
    }

    public Language CurrentLanguage { get; private set; }

    public event Action OnLanguageChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        int savedLanguage = PlayerPrefs.GetInt("GameLanguage", 0);

        CurrentLanguage = (Language)savedLanguage;
    }

    public void SetSpanish()
    {
        SetLanguage(Language.Spanish);
    }

    public void SetEnglish()
    {
        SetLanguage(Language.English);
    }

    private void SetLanguage(Language language)
    {
        CurrentLanguage = language;

        PlayerPrefs.SetInt("GameLanguage", (int)language);
        PlayerPrefs.Save();

        OnLanguageChanged?.Invoke();

        Debug.Log("Idioma cambiado a: " + language);
    }
}
