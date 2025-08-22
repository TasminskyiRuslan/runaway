using Unity.Cinemachine;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Settings")]
    [TagField]
    [SerializeField] private string enemyTag = "Enemy";

    private ChaseEnemy[] enemies;

    void Start()
    {
        gameObject.SetActive(false);
        SubscribeToEnemiesByTag();
    }

    private void OnDestroy()
    {
        if (enemies == null) return;

        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.OnGameOver -= HandleGameOver;
            }
        }
    }

    private void SubscribeToEnemiesByTag()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(enemyTag);
        enemies = new ChaseEnemy[enemyObjects.Length];

        for (int i = 0; i < enemyObjects.Length; i++)
        {
            enemies[i] = enemyObjects[i].GetComponent<ChaseEnemy>();
            if (enemies[i] != null)
            {
                enemies[i].OnGameOver += HandleGameOver;
            }
            else
            {
                Debug.LogWarning($"Objects {enemyObjects[i].name} with the tag {enemyTag} don't have the ChaseEnemy component");
            }
            Debug.Log($"{enemies.Length} enemies were found with the tag '{enemyTag}'");
        }
    }

    private void HandleGameOver()
    {
        ShowGameOverMenu();
    }

    public void ShowGameOverMenu()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
