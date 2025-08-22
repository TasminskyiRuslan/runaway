using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class ChaseEnemy : MonoBehaviour
{
    private enum EnemyState { Idle, Patrol, Chase }

    [Header("References")]
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Material[] materials;
    [SerializeField] private Renderer render;
    [SerializeField] private Transform player;

    [Header("Enemy Settings")]
    [SerializeField] private float detectionRadius = 20.0f;
    [SerializeField] private float hidingRadius = 10.0f;
    [SerializeField] private float catchRadius = 3.0f;
    [SerializeField] private float catchTime = 1.5f;
    [SerializeField] private float patrolRadius = 50.0f;
    [SerializeField] private float minPatrolWait = 0.5f;
    [SerializeField] private float maxPatrolWait = 4.0f;

    // Action field
    public event System.Action OnGameOver;

    // Chase and catch fields
    private NavMeshAgent agent;
    private EnemyState state = EnemyState.Idle;
    private float catchTimer = 0.0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.baseOffset = 1.0f;
        agent.speed = 4.0f;
        agent.stoppingDistance = 2.0f;
        agent.angularSpeed = 360.0f;
        agent.acceleration = 10.0f;
        render.sharedMaterial = materials[0];
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.Idle:
            case EnemyState.Patrol:
                DetectionPlayer();
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
        }
    }

    private void DetectionPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius && IsPlayerVisible())
        {
            StartChase();
        }
        else if (state != EnemyState.Patrol)
        {
            StartCoroutine(PatrolRoutine());
        }
    }

    private void StartChase()
    {
        state = EnemyState.Chase;
        catchTimer = 0.0f;
        StopAllCoroutines();
        render.sharedMaterial = materials[1];
    }

    private void ChasePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectionRadius || (distance > hidingRadius && !IsPlayerVisible()))
        {
            StopChase();
            return;
        }

        agent.SetDestination(player.position);
        CatchPlayer(distance);
    }

    private void StopChase()
    {
        state = EnemyState.Patrol;
        catchTimer = 0f;
        render.sharedMaterial = materials[0];
        StartCoroutine(PatrolRoutine());
    }

    private void CatchPlayer(float distance)
    {
        if (distance <= catchRadius)
        {
            catchTimer += Time.deltaTime;
            if (catchTimer >= catchTime)
            {
                OnGameOver?.Invoke();
            }
        }
        else
        {
            catchTimer = 0.0f;
        }
    }

    private bool IsPlayerVisible()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);
        return !Physics.Raycast(transform.position, dir, distance, obstacleMask);
    }

    private IEnumerator PatrolRoutine()
    {
        state = EnemyState.Patrol;

        while (state == EnemyState.Patrol)
        {
            render.sharedMaterial = materials[0];
            Vector3 patrolTarget = GetRandomNavSphere(transform.position, patrolRadius);
            agent.SetDestination(patrolTarget);

            float wait = Random.Range(minPatrolWait, maxPatrolWait);
            yield return new WaitForSeconds(wait);
        }
    }

    private Vector3 GetRandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randomDir = Random.insideUnitSphere * distance + origin;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, distance, -1);
        return hit.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hidingRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, catchRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
