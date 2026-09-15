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
    UnityEvent attackEvent;
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
            }
        }
    }
    void Update()
    {
        if (currentState == EnemyState.Chasing && currentChaseTime > 0)
        {
            currentChaseTime -= Time.deltaTime;

            if (playerGO != null)
            {
                Vector3 directionToPlayer = (playerGO.transform.position - transform.position).normalized;
                thisRigidbody.MovePosition(transform.position + directionToPlayer * Time.deltaTime * enemySpeed);
                this.transform.LookAt(playerGO.transform.position);
            }
        }
        else
        {
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
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            attackEvent.Invoke();
            currentState = EnemyState.Idle;
            attackEvent.RemoveListener(playerGO.SetDead);
            playerGO = null;
        }
    }
}
