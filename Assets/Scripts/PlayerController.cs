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

        // Determinar velocidad actual
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
            isRunning = false;
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

        if (inputDirection.magnitude >= 0.1f)
        {
            // 1. ÁNGULO DE MOVIMIENTO (360 grados, permite ir en reversa)
            float moveAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // 2. ÁNGULO DE ROTACIÓN (El truco: usamos Mathf.Abs en la Z para que NUNCA mire hacia atrás)
            float rotationAngle = Mathf.Atan2(inputDirection.x, Mathf.Abs(inputDirection.z)) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // Rotamos a Vera con el ángulo "mentiroso" para que mire al frente o a los lados
            Quaternion targetRotation = Quaternion.Euler(0f, rotationAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Movemos a Vera con el ángulo real (así puede retroceder)
            Vector3 moveDirection = Quaternion.Euler(0f, moveAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);
        }

        // Aplicamos gravedad
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}