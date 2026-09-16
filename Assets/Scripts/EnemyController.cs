using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EnemyController : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Chasing,
        Attacking
    }

    [SerializeField] Rigidbody thisRigidbody;

    PlayerDesguiseManager playerGO;

    EnemyState currentState = EnemyState.Idle;

    [SerializeField] float enemySpeed = 5f;
    [SerializeField] float chaseTimer = 10f;
    [SerializeField] List<Transform> waypoints = new List<Transform>();
    int waypointIndex = 0;

    [SerializeField] Transform currentWaypoint;
    float currentChaseTime = 0f;

    //// Posición a la que el enemigo fue alertado.
    Vector3 alertedPosition;

    //// Indica si el enemigo está yendo hacia una posición recibida por una cámara
    bool goingToAlertPosition = false;

    public UnityEvent attackEvent;

    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        if (attackEvent == null) attackEvent = new UnityEvent();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerGO = collider.GetComponent<PlayerDesguiseManager>();
            if (playerGO.currentDesguise != DesguiseType.Desguise)
            {
                attackEvent.AddListener(playerGO.SetDead);
                currentState = EnemyState.Chasing;
                currentChaseTime = chaseTimer;

                //// Yendo a posición recibida por cámara falso
                goingToAlertPosition = false;
            }
        }
    }
    void Update()
    {
        //// Si el enemigo está persiguiendo directamente al jugador
        if (currentState == EnemyState.Chasing && playerGO != null && !goingToAlertPosition)
        {
            currentChaseTime -= Time.deltaTime;

            if (currentChaseTime > 0)
            {
                Vector3 directionToPlayer = (playerGO.transform.position - transform.position).normalized;

                thisRigidbody.MovePosition(transform.position + directionToPlayer * Time.deltaTime * enemySpeed);

                this.transform.LookAt(new Vector3(playerGO.transform.position.x, transform.position.y, playerGO.transform.position.z));
            }
            else
            {
                currentState = EnemyState.Idle;
                playerGO = null;
            }

            return;
        }

        //// Si el enemigo fue alertado por una cámara se mueve hacia la última posición conocida del jugador
        if (currentState == EnemyState.Chasing && goingToAlertPosition)
        {
            currentChaseTime -= Time.deltaTime;

            if (currentChaseTime > 0)
            {
                Vector3 directionToAlert = (alertedPosition - transform.position).normalized;

                Vector3 movementDirection = new Vector3(directionToAlert.x, 0, directionToAlert.z).normalized;

                thisRigidbody.MovePosition(transform.position + movementDirection * Time.deltaTime * enemySpeed);

                this.transform.LookAt(new Vector3(alertedPosition.x, transform.position.y, alertedPosition.z));

                //// Si el enemigo llegó aproximadamente a la posición alertada deja de perseguirla
                if (Vector3.Distance(transform.position, alertedPosition) < 1f)
                {
                    goingToAlertPosition = false;
                    currentState = EnemyState.Idle;
                }
            }
            else
            {
                goingToAlertPosition = false;
                currentState = EnemyState.Idle;
            }

            return;
        }

        //// Comportamiento normal de patrulla
        currentState = EnemyState.Idle;
        playerGO = null;
        if (waypoints.Count > 0)
        {
            currentWaypoint = waypoints[waypointIndex];
            if (Vector3.Distance(transform.position, currentWaypoint.position) < 1.5f)
            {
                waypointIndex = (waypointIndex + 1) % waypoints.Count;
                currentWaypoint = waypoints[waypointIndex];
            }
            Vector3 directionToWaypoint = (currentWaypoint.position - transform.position).normalized;
            Vector3 vector3ToWaypoint = new Vector3(directionToWaypoint.x, 0, directionToWaypoint.z).normalized;
            thisRigidbody.MovePosition(transform.position + vector3ToWaypoint * Time.deltaTime * enemySpeed);
            this.transform.LookAt(new Vector3(currentWaypoint.position.x, transform.position.y, currentWaypoint.position.z));
        }
    }


    //// Permite que una cámara de seguridad alerte al enemigo de la posición del jugador
    public void AlertAtPosition(Vector3 position)
    {
        //// Guarda la posición donde la cámara vio al jugador
        alertedPosition = position;

        //// Indica que el enemigo debe dirigirse hacia esa posición
        goingToAlertPosition = true;

        //// Cambia el estado del enemigo a Chasing
        currentState = EnemyState.Chasing;

        //// Tiempo limitado para llegar a la posición
        currentChaseTime = chaseTimer;

        //// Ya no necesita perseguir directamente al jugador
        playerGO = null;

        Debug.Log(gameObject.name + " fue alertado de la posición del jugador.");
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            attackEvent.Invoke();
            currentState = EnemyState.Idle;

            if (playerGO != null)
            {
                attackEvent.RemoveListener(playerGO.SetDead);
            }

            playerGO = null;
            goingToAlertPosition = false;
        }
    }
}
