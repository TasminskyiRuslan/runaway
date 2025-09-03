using System.Collections;
using Unity.AI.Navigation;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private enum EnemyState { Idle, Patrol, Chase }

    [Header("References")]
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private Material[] _materials;
    [SerializeField] private Renderer _body;
    [SerializeField] private Transform _player;
    [SerializeField] private PlayerHide _playerHide;

    [Header("Enemy Settings")]
    [SerializeField] private float _detectionRadius = 20.0f;
    [SerializeField] private float _hidingRadius = 10.0f;
    [SerializeField] private float _catchRadius = 3f;
    [SerializeField] private float _catchTime = 2f;
    [SerializeField] private float _patrolRadius = 50.0f;
    [SerializeField] private float _minPatrolWait = 0.5f;
    [SerializeField] private float _maxPatrolWait = 4.0f;

    // Action field
    public event System.Action OnGameOver;

    // Chase and catch fields
    private NavMeshAgent _agent;
    private EnemyState _state = EnemyState.Idle;
    private float _catchTimer = 0.0f;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.baseOffset = 1.0f;
        _agent.speed = 4.0f;
        _agent.stoppingDistance = 2.0f;
        _agent.angularSpeed = 360.0f;
        _agent.acceleration = 10.0f;
        _body.sharedMaterial = _materials[0];
        _obstacleMask = LayerMask.GetMask("Obstacle");
        _playerHide = PlayerHide.Instance;
        if (_playerHide != null)
        {
            _player = _playerHide.transform;
        }
    }

    private void Update()
    {
        switch (_state)
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
        float distance = Vector3.Distance(transform.position, _player.position);

        if ((distance <= _detectionRadius && !_playerHide.IsHidden && IsPlayerVisible()))
        {
            StartChase();
        }
        else if (_state != EnemyState.Patrol)
        {
            StartCoroutine(PatrolRoutine());
        }
    }


    private void StartChase()
    {
        if (_state == EnemyState.Chase) return;

        _state = EnemyState.Chase;
        _catchTimer = 0.0f;
        StopAllCoroutines();
        _body.sharedMaterial = _materials[1];
    }

    private void ChasePlayer()
    {
        Vector3 target;

        if (!_playerHide.IsHidden && Vector3.Distance(transform.position, _player.position) <= _detectionRadius)
        {
            target = _player.position;
        }
        else if (_playerHide.IsHidden && _playerHide.currentSpot != null && Vector3.Distance(transform.position, _playerHide.currentSpot.exitPoint.position) <= _hidingRadius)
        {
            target = _playerHide.currentSpot.exitPoint.position;
        }
        else
        {
            StopChase();
            return;
        }

        if (_agent.destination != target)
            _agent.SetDestination(target);

        float distanceToTarget = Vector3.Distance(transform.position, target);
        CatchPlayer(distanceToTarget);
    }


    private void CatchPlayer(float distance)
    {
        if (distance <= _catchRadius)
        {
            _catchTimer += Time.deltaTime;

            if (_catchTimer >= _catchTime)
            {
                //if (_playerHide.IsHidden)
                //{
                //    HandleGameOver();

                //}
                //else
                //{
                //    HandleGameOver();
                //}
                HandleGameOver();
            }
        }
        else
        {
            _catchTimer = 0f;
        }
    }

    public void StopChase()
    {
        if (_state == EnemyState.Patrol) return;

        _state = EnemyState.Patrol;
        _catchTimer = 0f;
        _body.sharedMaterial = _materials[0];
        StartCoroutine(PatrolRoutine());
    }

    public void HandleGameOver()
    {
        OnGameOver?.Invoke();
        StopChase();
    }

    private bool IsPlayerVisible()
    {
        Vector3 dir = (_player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, _player.position);
        return !Physics.Raycast(transform.position, dir, distance, _obstacleMask);
    }

    private IEnumerator PatrolRoutine()
    {
        _state = EnemyState.Patrol;

        while (_state == EnemyState.Patrol)
        {
            _body.sharedMaterial = _materials[0];
            Vector3 patrolTarget = GetRandomNavSphere(transform.position, _patrolRadius);

            if (_agent.destination != patrolTarget)
                _agent.SetDestination(patrolTarget);

            float wait = Random.Range(_minPatrolWait, _maxPatrolWait);
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
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _hidingRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _catchRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _patrolRadius);
    }


}