using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class ChaseEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private Material[] _materials;
    [SerializeField] private Renderer _render;
    [SerializeField] private Transform _player;

    [Header("Chase settings")]
    [SerializeField] private float _detectionRadius = 20.0f;
    [SerializeField] private float _wanderRadius = 50.0f;
    [SerializeField] private float _minWanderWait = 0.5f;
    [SerializeField] private float _maxWanderWait = 4.0f;

    private NavMeshAgent _agent;

    private bool _isChasing;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.baseOffset = 1.0f;
        _agent.speed = 4.0f;
        _agent.stoppingDistance = 2.0f;
        _agent.angularSpeed = 360.0f;
        _agent.acceleration = 10.0f;
        _render.enabled = true;
        _render.sharedMaterial = _materials[0];
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
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer <= _detectionRadius && IsPlayerVisible())
        {
            if (!_isChasing)
            {
                StartChase();
            }
        }
    }

    private void Chase()
    {
        if ((Vector3.Distance(transform.position, _player.position) > _detectionRadius) && !IsPlayerVisible())
        {
            StopChase();
        }
        else
        {
            _render.sharedMaterial = _materials[1];
            _agent.SetDestination(_player.position);
        }
    }

    private bool IsPlayerVisible()
    {
        Vector3 directionToPlayer = (_player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        return !Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, _obstacleMask);
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
            _render.sharedMaterial = _materials[0];
            Vector3 wanderTarget = GetRandomNavSpherePosition(transform.position, _wanderRadius);
            _agent.SetDestination(wanderTarget);

            float waitTime = Random.Range(_minWanderWait, _maxWanderWait);
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
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _wanderRadius);
    }
}