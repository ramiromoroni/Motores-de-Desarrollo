using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject settingsPanel;

    public void OpenSettings()
    {
        mainMenuUI.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuUI.SetActive(true);
    }
}
