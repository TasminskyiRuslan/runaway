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
    [TagField][SerializeField] private string _playerTag = "Player";
    [TagField][SerializeField] private string _surfaceTag = "Surface";

    [Header("Spawn Settings")]
    [SerializeField] private float minDistanceFromPlayer = 40; // м≥н≥мальна в≥дстань в≥д гравц€
    [SerializeField] private float minDistanceBetweenItems = 30; // м≥н≥мальна в≥дстань м≥ж предметами

    private Collider _navMeshSurface;

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
        List<Vector3> candidatePoints = new List<Vector3>();

        // √енеруЇмо с≥тку з кроком = м≥н≥мальна дистанц≥€
        for (float x = _navMeshSurface.bounds.min.x; x < _navMeshSurface.bounds.max.x; x += minDistanceBetweenItems)
        {
            for (float z = _navMeshSurface.bounds.min.z; z < _navMeshSurface.bounds.max.z; z += minDistanceBetweenItems)
            {
                Vector3 pos = new Vector3(x, _navMeshSurface.bounds.max.y + 10f, z);

                if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
                {
                    // ≥гноруЇмо точки близько до гравц€
                    if (Vector3.Distance(hit.position, _player.position) < minDistanceFromPlayer)
                        continue;

                    candidatePoints.Add(hit.position);
                }
            }
        }

        // ѕерем≥шуЇмо точки
        Shuffle(candidatePoints);

        // —павнимо ун≥кальн≥ предмети
        int count = Mathf.Min(_clothingPrefabs.Length, candidatePoints.Count);
        for (int i = 0; i < count; i++)
        {
            GameObject prefab = _clothingPrefabs[i];
            Instantiate(prefab, candidatePoints[i], Quaternion.Euler(0, Random.Range(0, 360f), 0));
        }
    }

    // FisherЦYates shuffle
    private void Shuffle(List<Vector3> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
