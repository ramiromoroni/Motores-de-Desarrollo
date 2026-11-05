
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LanguageButton : MonoBehaviour
{
    public bool selectSpanish = true;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ChangeLanguage);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ChangeLanguage);
    }

    private void ChangeLanguage()
    {
        if (LanguageManager.Instance == null)
        {
            Debug.LogError("No se encontró LanguageManager.");
            return;
        }

        if (selectSpanish)
        {
            LanguageManager.Instance.SetSpanish();
        }
        else
        {
            LanguageManager.Instance.SetEnglish();
        }
    }
}
