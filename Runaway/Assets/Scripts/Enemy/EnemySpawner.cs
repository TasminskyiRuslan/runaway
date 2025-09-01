//using Unity.AI.Navigation;
//using Unity.Cinemachine;
//using UnityEngine;
//using UnityEngine.AI;

//public class EnemySpawnerOnSurface : MonoBehaviour
//{
//    [Header("References")]
//    [SerializeField] private GameObject _enemyPrefab;
//    [SerializeField] private NavMeshSurface _navMeshSurface;
//    [TagField]
//    [SerializeField] private string _surfaceTag = "Surface";

//    [Header("Spawn Settings")]
//    [SerializeField] private int _enemyCount = 5;

//    private void Awake()
//    {
//        _navMeshSurface = GameObject.FindGameObjectWithTag(_surfaceTag)?.GetComponent<NavMeshSurface>();
//    }

//    private void Start()
//    {
//        for (int i = 0; i < _enemyCount; i++)
//        {
//            SpawnEnemy();
//        }
//    }

//    private void SpawnEnemy()
//    {
//        var bounds = _navMeshSurface.navMeshData.sourceBounds;
//        Vector3 randomPos = new Vector3(
//            Random.Range(bounds.min.x, bounds.max.x),
//            bounds.center.y,
//            Random.Range(bounds.min.z, bounds.max.z)
//        );

//        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 100f, NavMesh.AllAreas))
//        {
//            Instantiate(_enemyPrefab, hit.position, Quaternion.identity);
//        }
//    }
//}


using Unity.AI.Navigation;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnerOnSurface : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private Transform _playerTransform;
    [TagField]
    [SerializeField] private string _surfaceTag = "Surface";

    [Header("Spawn Settings")]
    [SerializeField] private int _enemyCount = 5;
    [SerializeField] private float _spawnDistance = 15f; // ‘≥ксована в≥дстань в≥д гравц€
    //[SerializeField] private float _heightOffset = 10f; // ¬исота дл€ пошуку NavMesh

    private void Awake()
    {
        _navMeshSurface = GameObject.FindGameObjectWithTag(_surfaceTag)?.GetComponent<NavMeshSurface>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Start()
    {
        for (int i = 0; i < _enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        // √енеруЇмо точку на кол≥ навколо гравц€
        float angle = Random.Range(0f, 360f);
        Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        Vector3 spawnCirclePos = _playerTransform.position + direction * _spawnDistance;

        // ѕ≥дн≥маЇмо точку дл€ пошуку NavMesh
        Vector3 searchPos = spawnCirclePos + Vector3.up;

        // ћоже не знайти NavMesh, €кщо точка п≥д землею
        if (NavMesh.SamplePosition(spawnCirclePos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            Instantiate(_enemyPrefab, hit.position, Quaternion.identity);
        }
        else
        {
            // якщо не знайшли на NavMesh, створюЇмо просто на висот≥
            Instantiate(_enemyPrefab, spawnCirclePos, Quaternion.identity);
        }
    }
}