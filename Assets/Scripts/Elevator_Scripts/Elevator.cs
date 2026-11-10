using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] float riseHeight = 13.751f;  // cuánto sube
    [SerializeField] float travelDuration = 4f;   // cuánto tarda el viaje
    [SerializeField] GameObject player;           // Vera, para llevarla con la plataforma

    private bool isUp = false;     // ¿está arriba?
    private bool isMoving = false; // ¿está viajando?
    public bool IsUp => isUp;         // otros scripts pueden saber si está arriba
    public bool IsMoving => isMoving; // otros scripts pueden saber si está viajando
    private Vector3 downPosition;
    private Vector3 upPosition;
    private PlayerController playerController;
    private CharacterController playerCharacter;

    void Start()
    {
        // guardamos la posición de abajo y calculamos la de arriba
        downPosition = transform.localPosition;
        upPosition = downPosition + Vector3.up * riseHeight;

        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            playerCharacter = player.GetComponent<CharacterController>();
        }
    }

    // botón de la plataforma: sube o baja llevando a Vera
    [ContextMenu("Use")]
    public void Use()
    {
        if (isMoving) return;
        Vector3 target = isUp ? downPosition : upPosition;
        StartCoroutine(MoveRoutine(target, true));
    }

    // botón de llamada: si está arriba, baja sin Vera
    [ContextMenu("Call")]
    public void Call()
    {
        if (isMoving || !isUp) return;
        StartCoroutine(MoveRoutine(downPosition, false));
    }

    IEnumerator MoveRoutine(Vector3 target, bool carryPlayer)
    {
        isMoving = true;
        if (carryPlayer) SetPlayerControl(false); // Vera se queda quieta

        Vector3 start = transform.localPosition;
        float time = 0f;

        while (time < travelDuration)
        {
            time += Time.deltaTime;
            Vector3 previous = transform.position;
            transform.localPosition = Vector3.Lerp(start, target, time / travelDuration);

            // movemos a Vera lo mismo que se movió la plataforma
            if (carryPlayer && player != null)
                player.transform.position += transform.position - previous;

            yield return null;
        }

        isUp = target == upPosition;
        if (carryPlayer) SetPlayerControl(true); // Vera vuelve a moverse
        isMoving = false;
    }

    // apaga o prende el movimiento de Vera
    void SetPlayerControl(bool active)
    {
        if (playerController != null) playerController.enabled = active;
        if (playerCharacter != null) playerCharacter.enabled = active;
    }
}