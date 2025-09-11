using Unity.AI.Navigation;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _playerTransform;

    [Header("Spawn Settings")]
    [SerializeField] private int _enemyCount = 5;
    [SerializeField] private float _spawnDistance = 15f; 

    private void Start()
    {
        _playerTransform = PlayerInstance.Instance.transform;
        for (int i = 0; i < _enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        float angle = Random.Range(0f, 360f);
        Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        Vector3 spawnCirclePos = _playerTransform.position + direction * _spawnDistance;

        Vector3 searchPos = spawnCirclePos + Vector3.up;

        if (NavMesh.SamplePosition(spawnCirclePos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            Instantiate(_enemyPrefab, hit.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_enemyPrefab, spawnCirclePos, Quaternion.identity);
        }
    }
}