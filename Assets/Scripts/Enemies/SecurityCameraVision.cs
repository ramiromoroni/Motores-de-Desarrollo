using UnityEngine;

public class SecurityCameraVision : MonoBehaviour
{
    //// Por alguna razon no puedo hacer que la camara funcione sin separarla en 2 scripts
    [SerializeField] SecurityCamera securityCamera;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            securityCamera.PlayerEnteredVision(other.GetComponent<PlayerDesguiseManager>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            securityCamera.PlayerExitedVision();
        }
    }
}