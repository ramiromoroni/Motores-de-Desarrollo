using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [Header("Configuración de Velocidad y Angulo")]
    [SerializeField] float rotationSpeed = 40f;

    [SerializeField] float rotationAngle = 45f;

    [Header("Radio de Alerta")]
    [SerializeField] float alertRadius = 15f;

    [SerializeField] LayerMask obstacleLayers;
    [SerializeField] GameSceneManager gameSceneManager;

    float startingRotationY;

    PlayerDesguiseManager playerGO;

    bool playerInsideVision = false;

    bool alertTriggered = false;


    void Start()
    {
        startingRotationY = transform.eulerAngles.y;

        if (gameSceneManager == null)
        {
            gameSceneManager = FindFirstObjectByType<GameSceneManager>();
        }
    }


    void Update()
    {
        MoveCamera();

        CheckPlayer();
    }


    //// Controla el movimiento la cámara
    void MoveCamera()
    {
        float angle = Mathf.Sin(Time.time * rotationSpeed * Mathf.Deg2Rad) * rotationAngle;

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, startingRotationY + angle, transform.eulerAngles.z);
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = playerGO.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        //// Funcion debug para co´mprobar que esta porqueria funciona
        Debug.DrawRay(transform.position, directionToPlayer.normalized * distanceToPlayer, Color.red);

        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, distanceToPlayer, obstacleLayers))
        {
            return false;
        }

        return true;
    }

    //// Funcion llamada por SecurityCameraVision cuando el jugador entra en VisionArea
    public void PlayerEnteredVision(PlayerDesguiseManager player)
    {
        playerGO = player;

        if (playerGO != null)
        {
            playerInsideVision = true;

            Debug.Log("La cámara detectó al jugador.");
        }
    }


    //// Funcion llamada por SecurityCameraVision cuando el jugador sale de VisionArea
    public void PlayerExitedVision()
    {
        playerInsideVision = false;

        playerGO = null;

        //// Para que la cámara pueda volver a alertar si el jugador vuelve a entrar sin disfraz
        alertTriggered = false;

        Debug.Log("El jugador salió del área de visión.");
    }

    void CheckPlayer()
    {
        if (alertTriggered)
        {
            return;
        }

        if (!playerInsideVision || playerGO == null)
        {
            return;
        }

        if (playerGO.currentDesguise == DesguiseType.Desguise)
        {
            return;
        }

        if (!CanSeePlayer())
        {
            return;
        }

        alertTriggered = true;
        //// Alerta a los enemigos cercanos
        AlertNearbyEnemies(playerGO.transform.position);
    }


    //// Busca enemigos dentro del radio de alerta y les comunica la posición del jugador
    void AlertNearbyEnemies(Vector3 playerPosition)
    {
        //// Busca todos los Colliders que se encuentren dentro del radio
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, alertRadius);

        //// Para evitar alertar dos veces al mismo enemigo si en un futuro le agregamos más Colliders
        HashSet<EnemyController> enemies = new HashSet<EnemyController>();

        foreach (Collider collider in nearbyColliders)
        {
            EnemyController enemy = collider.GetComponentInParent<EnemyController>();

            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }

        //// Avisa a todos los enemigos encontrados
        foreach (EnemyController enemy in enemies)
        {
            enemy.AlertAtPosition(playerPosition);
        }

        Debug.Log("La cámara alertó a " + enemies.Count + " enemigo(s).");
    }


    //// Dibuja el radio de alerta en Scene (Activar gizmos en al ventana Scene y seleccionar las camaras)
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }

}