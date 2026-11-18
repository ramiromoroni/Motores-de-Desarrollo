using TMPro;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI promptText;
    [SerializeField] string dialogText;
    private bool used = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (used) return;
            promptText.text = dialogText;
            promptText.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            promptText.text = "";
            promptText.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
