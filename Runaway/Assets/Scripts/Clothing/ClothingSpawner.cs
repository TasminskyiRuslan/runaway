using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class ClothingSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] _clothingPrefabs;
    [SerializeField] private Transform _player;
    [TagField]
    [SerializeField] private string _playerTag = "Player";
    [TagField]
    [SerializeField] private string _surfaceTag = "Surface";

    [Header("Spawn Settings")]
    [SerializeField] private float minDistanceFromPlayer = 10f;
    //[SerializeField] private float edgePadding = 2f;

    private Collider _navMeshSurface;
    private List<Vector3> spawnedPositions = new List<Vector3>();

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(_playerTag).transform;
        _navMeshSurface = GameObject.FindGameObjectWithTag(_surfaceTag).GetComponent<Collider>();
    }

    void Start()
    {
        if (_navMeshSurface != null)
        {
            SpawnClothing();
        }
        else
        {
            Debug.LogError("NavMeshSurface не найден на сцене!");
        }
    }

    void SpawnClothing()
    {
        int spawnedCount = 0;
        int attempts = 0;
        while (spawnedCount < _clothingPrefabs.Length && attempts < 1000)
        {
            attempts++;
            Vector3 randomPos = new Vector3(
                Random.Range(_navMeshSurface.bounds.min.x, _navMeshSurface.bounds.max.x),
                _navMeshSurface.bounds.max.y + 10f,
                Random.Range(_navMeshSurface.bounds.min.z, _navMeshSurface.bounds.max.z)
            );

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                Vector3 finalPos = hit.position;

                if (Vector3.Distance(finalPos, _player.position) < minDistanceFromPlayer)
                    continue;

                bool tooClose = false;
                foreach (var pos in spawnedPositions)
                {
                    if (Vector3.Distance(finalPos, pos) < 2f) // мінімальна дистанція
                    {
                        tooClose = true;
                        break;
                    }
                }
                if (tooClose) continue;

                GameObject prefab = _clothingPrefabs[spawnedCount]; // унікальний
                Instantiate(prefab, finalPos, Quaternion.Euler(0, Random.Range(0, 360f), 0));
                spawnedPositions.Add(finalPos);
                spawnedCount++;
            }
        }
    }
}
