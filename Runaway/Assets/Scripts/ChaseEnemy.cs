using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class ChaseEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Material[] materials;
    [SerializeField] private Renderer render;

    [Header("Chase settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRadius = 20.0f;
    [SerializeField] private float wanderRadius = 50.0f;
    [SerializeField] private float minWanderWait = 0.5f;
    [SerializeField] private float maxWanderWait = 4.0f;

    private NavMeshAgent _agent;

    private bool _isChasing;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.baseOffset = 1.0f;
        _agent.speed = 4.5f;
        _agent.stoppingDistance = 2.0f;
        _agent.angularSpeed = 360.0f;
        _agent.acceleration = 10.0f;
        render.enabled = true;
        render.sharedMaterial = materials[0];
    }

    private void Update()
    {
        PlayerDetection();
        if (_isChasing)
        {
            Chase();
        }
        else if (!_agent.hasPath)
        {
            StartCoroutine(WanderRoutine());
        }
    }

    private void PlayerDetection()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius && IsPlayerVisible())
        {
            if (!_isChasing)
            {
                StartChase();
            }
        }
    }

    private void Chase()
    {
        if ((Vector3.Distance(transform.position, player.position) > detectionRadius) && !IsPlayerVisible())
        {
            StopChase();
        }
        else
        {
            render.sharedMaterial = materials[1];
            _agent.SetDestination(player.position);
        }
    }

    private bool IsPlayerVisible()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return !Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask);
    }

    private void StartChase()
    {
        _isChasing = true;
        StopCoroutine(WanderRoutine());
    }

    private void StopChase()
    {
        _isChasing = false;
        StartCoroutine(WanderRoutine());
    }

    private IEnumerator WanderRoutine()
    {
        while (!_isChasing)
        {
            render.sharedMaterial = materials[0];
            Vector3 wanderTarget = GetRandomNavSpherePosition(transform.position, wanderRadius);
            _agent.SetDestination(wanderTarget);

            float waitTime = Random.Range(minWanderWait, maxWanderWait);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private Vector3 GetRandomNavSpherePosition(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);
        return navHit.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}