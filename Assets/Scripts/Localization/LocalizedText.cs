
using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    [Header("Textos")]
    public string spanishText;
    public string englishText;

    private TMP_Text textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged += UpdateText;
        }

        UpdateText();
    }

    private void OnDisable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged -= UpdateText;
        }
    }

    public void UpdateText()
    {
        if (textComponent == null)
            return;

        if (LanguageManager.Instance == null)
            return;

        if (LanguageManager.Instance.CurrentLanguage ==
            LanguageManager.Language.Spanish)
        {
            textComponent.text = spanishText;
        }
        else
        {
            textComponent.text = englishText;
        }
    }
}
