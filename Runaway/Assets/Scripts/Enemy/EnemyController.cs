using System.Collections;
using Unity.AI.Navigation;
using Unity.Cinemachine;
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
    [TagField]
    [SerializeField] private string _playerTag = "Player";

    [Header("Enemy Settings")]
    [SerializeField] private float _detectionRadius = 17.0f;
    [SerializeField] private float _catchRadius = 3.0f;
    [SerializeField] private float _catchTime = 1.5f;
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
        _player = GameObject.FindGameObjectWithTag(_playerTag)?.transform;
        _playerHide = GameObject.FindGameObjectWithTag(_playerTag)?.GetComponent<PlayerHide>();
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

        if (distance <= _detectionRadius && !_playerHide.IsHidden)
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
        _state = EnemyState.Chase;
        _catchTimer = 0.0f;
        StopAllCoroutines();
        _body.sharedMaterial = _materials[1];
    }

    private void ChasePlayer()
    {
        float distance = Vector3.Distance(transform.position, _player.position);

        if (distance > _detectionRadius || _playerHide.IsHidden)
        {
            StopChase();
            return;
        }

        _agent.SetDestination(_player.position);
        CatchPlayer(distance);
    }

    private void StopChase()
    {
        _state = EnemyState.Patrol;
        _catchTimer = 0f;
        _body.sharedMaterial = _materials[0];
        StartCoroutine(PatrolRoutine());
    }

    private void CatchPlayer(float distance)
    {
        if (distance <= _catchRadius)
        {
            _catchTimer += Time.deltaTime;
            if (_catchTimer >= _catchTime)
            {
                OnGameOver?.Invoke();
            }
        }
        else
        {
            _catchTimer = 0.0f;
        }
    }

    private IEnumerator PatrolRoutine()
    {
        _state = EnemyState.Patrol;

        while (_state == EnemyState.Patrol)
        {
            _body.sharedMaterial = _materials[0];
            Vector3 patrolTarget = GetRandomNavSphere(transform.position, _patrolRadius);
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _catchRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _patrolRadius);
    }
}
