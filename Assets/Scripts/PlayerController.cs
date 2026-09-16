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

    [Header("Configuración de Velocidades")]
    [SerializeField] private float walkSpeed = 5.0f;
    [SerializeField] private float runSpeed = 8.0f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f; // Ajustado para un giro más suave

    [Header("Configuración de Agacharse")]
    [SerializeField] private float normalHeight = 2.0f;
    [SerializeField] private float crouchHeight = 1.0f;
    private Vector3 normalCenter;
    private Vector3 crouchCenter = new Vector3(0, -0.5f, 0); // Baja el centro para que no flote

    [Header("Estados Públicos (Para el EnemyController)")]
    public bool isRunning = false;
    public bool isCrouching = false;

    private float currentSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        currentSpeed = walkSpeed;

        // Guardamos el centro original de la cápsula
        normalCenter = controller.center;
    }

    // EVENTOS DEL INPUT SYSTEM 
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started) isRunning = true;
        if (context.canceled) isRunning = false;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isCrouching = true;
            controller.height = crouchHeight; // Achica la cápsula a la mitad
            controller.center = crouchCenter;
        }
        if (context.canceled)
        {
            isCrouching = false;
            controller.height = normalHeight; // Vuelve a la altura normal
            controller.center = normalCenter;
        }
    }

    // LÓGICA DE MOVIMIENTO 
    void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //  Determinar velocidad actual (Prioridad: Agacharse > Correr > Caminar)
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
            isRunning = false; // Bloquea correr si estás agachado
        }
        else if (isRunning)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // Solo rotamos y movemos si estamos apretando teclas
        if (inputDirection.magnitude >= 0.1f)
        {
            //  Calculamos hacia dónde debe mirar según la cámara
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            //  Rotamos al personaje suavemente
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Movemos al personaje en esa dirección usando la velocidad actual
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);
        }

        // Aplicamos gravedad
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}