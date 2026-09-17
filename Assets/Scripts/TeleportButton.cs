using UnityEngine;

public class TeleportButton : MonoBehaviour, Iinteractable
{
    [SerializeField] Transform teleportPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador dentro del botón");
            other.GetComponent<InteractionManager>().SubscribeListener(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador salió del botón");
            other.GetComponent<InteractionManager>().UnsubscribeListener(this);
        }
    }

    public void Interact()
    {
        Debug.Log("TELETRANSPORTANDO JUGADOR");

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.Log("NO SE ENCONTRÓ EL PLAYER");
            return;
        }

        if (teleportPoint == null)
        {
            Debug.Log("NO HAY TELEPORT POINT ASIGNADO");
            return;
        }

        CharacterController playerController = player.GetComponent<CharacterController>();

        if (playerController == null)
        {
            Debug.Log("EL PLAYER NO TIENE CHARACTER CONTROLLER");
            return;
        }

        InteractionManager interactionManager = player.GetComponent<InteractionManager>();

        if (interactionManager != null)
        {
            interactionManager.UnsubscribeListener(this);
        }

        playerController.enabled = false;
        player.transform.position = teleportPoint.position;
        playerController.enabled = true;

        Debug.Log("PLAYER TELETRANSPORTADO A: " + teleportPoint.position);
    }
}