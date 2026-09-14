using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float gravityValue = -9.81f;

    void Start()
    {
        // Obtenemos la referencia al componente, como pide la consigna
        controller = GetComponent<CharacterController>();
    }

    // Esta función recibe los datos del New Input System (fases: started, performed, cancelled)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        // Verificamos si estamos tocando el piso
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f; // Reseteamos la velocidad vertical si tocamos el suelo
        }

        // Creamos un vector de movimiento usando los ejes X y Z
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Movemos el personaje
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Si hay movimiento, rotamos al personaje hacia donde camina usando transform.forward
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Aplicamos gravedad constante
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
