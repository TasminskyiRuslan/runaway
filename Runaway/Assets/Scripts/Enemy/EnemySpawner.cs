using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _playerTransform;
    [TagField]
    [SerializeField] private string _surfaceTag = "Surface";

    [Header("Spawn Settings")]
    [SerializeField] private int _enemyCount = 5;
    [SerializeField] private float _minDistanceFromPlayer = 10f;
    [SerializeField] private float _minDistanceBetweenEnemies = 6f;

    private Collider _navMeshSurface;

    private void Awake()
    {
        _navMeshSurface = GameObject.FindGameObjectWithTag(_surfaceTag).GetComponent<Collider>();
    }

    private void Start()
    {
        _playerTransform = PlayerInstance.Instance.transform;
        if (_navMeshSurface != null)
        {
            SpawnEnemies();
        }
        else
        {
            Debug.LogError("NavMeshSurface not found!");
        }
    }

    private void SpawnEnemies()
    {
        List<Vector3> candidatePoints = new List<Vector3>();

        for (float x = _navMeshSurface.bounds.min.x; x < _navMeshSurface.bounds.max.x; x += _minDistanceBetweenEnemies)
        {
            for (float z = _navMeshSurface.bounds.min.z; z < _navMeshSurface.bounds.max.z; z += _minDistanceBetweenEnemies)
            {
                Vector3 pos = new Vector3(x, _navMeshSurface.bounds.max.y + 10f, z);

                if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
                {
                    if (Vector3.Distance(hit.position, _playerTransform.position) < _minDistanceFromPlayer)
                        continue;

                    candidatePoints.Add(hit.position);
                }
            }
        }

        Shuffle(candidatePoints);

        int count = Mathf.Min(_enemyCount, candidatePoints.Count);
        for (int i = 0; i < count; i++)
        {
            GameObject enemyObj = Instantiate(
                _enemyPrefab,
                candidatePoints[i],
                Quaternion.Euler(0, Random.Range(0, 360f), 0)
            );

            // ?? Реєстрація ворога після спавну
            EnemyController enemy = enemyObj.GetComponent<EnemyController>();
            if (enemy != null)
            {
                GameOverMenuManager.Instance.RegisterEnemy(enemy);
            }
        }
    }

    private void Shuffle(List<Vector3> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
