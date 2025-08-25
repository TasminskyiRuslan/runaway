using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnerOnSurface : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private int _enemyCount = 5;

    private void Awake()
    {
        _navMeshSurface = GameObject.FindGameObjectWithTag("Surface")?.GetComponent<NavMeshSurface>();

        if (!_navMeshSurface)
            Debug.LogError("NavMeshSurface not found");
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
        var bounds = _navMeshSurface.navMeshData.sourceBounds;
        Vector3 randomPos = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.center.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );

        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 100f, NavMesh.AllAreas))
        {
            Instantiate(_enemyPrefab, hit.position, Quaternion.identity);
        }
    }
}
