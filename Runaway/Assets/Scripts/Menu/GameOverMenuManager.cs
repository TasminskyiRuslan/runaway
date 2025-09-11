using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuManager : MonoBehaviour
{
    public static GameOverMenuManager Instance { get; private set; }

    [Header("References")]
    [TagField]
    [SerializeField] private string _enemyTag = "Enemy";
    [SerializeField] private GameObject _gameOverMenuPanel;

    // Enemies field
    private EnemyController[] _enemies;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _gameOverMenuPanel.SetActive(false);
        SubscribeToEnemiesByTag();
    }

    private void OnDestroy()
    {
        if (_enemies == null) return;

        foreach (var enemy in _enemies)
        {
            if (enemy != null)
            {
                enemy.OnGameOver -= HandleGameOver;
            }
        }
    }

    private void SubscribeToEnemiesByTag()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(_enemyTag);
        _enemies = new EnemyController[enemyObjects.Length];

        for (int i = 0; i < enemyObjects.Length; i++)
        {
            _enemies[i] = enemyObjects[i].GetComponent<EnemyController>();
            if (_enemies[i] != null)
            {
                _enemies[i].OnGameOver += HandleGameOver;
            }
            else
            {
                Debug.LogWarning($"Objects {enemyObjects[i].name} with the tag {_enemyTag} don't have the ChaseEnemy component");
            }
            Debug.Log($"{_enemies.Length} enemies were found with the tag '{_enemyTag}'");
        }
    }

    private void HandleGameOver()
    {
        ShowGameOverMenu();
    }

    public void ShowGameOverMenu()
    {
        _gameOverMenuPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void OnRestartButtonClicked()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMenuButtonClicked(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
