using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    // Referencia a la cámara para que el movimiento sea relativo a ella
    private Transform cameraTransform;

    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float gravityValue = -9.81f;

    // Nueva variable para que Vera gire suavemente
    [SerializeField] private float rotationSpeed = 10f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Vinculamos automáticamente la Main Camera del juego
        cameraTransform = Camera.main.transform;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // Solo rotamos y movemos si estamos apretando teclas
        if (inputDirection.magnitude >= 0.1f)
        {
            // 1. Calculamos hacia dónde debe mirar según la cámara
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // 2. Rotamos al personaje suavemente (así le podés ver la nariz al darse vuelta)
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 3. Movemos al personaje en esa dirección
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
        }

        // Aplicamos gravedad
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}