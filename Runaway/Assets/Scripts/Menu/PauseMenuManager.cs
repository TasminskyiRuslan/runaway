using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _pauseButton;

    public bool IsPaused { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _pauseMenuPanel.SetActive(false);
        _pauseButton.SetActive(true);
    }

    public void OnPauseButtonClicked()
    {
        if (IsPaused) return;

        IsPaused = true;
        _pauseMenuPanel.SetActive(true);
        _pauseButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void OnResumeButtonClicked()
    {
        if (!IsPaused) return;

        IsPaused = false;
        _pauseMenuPanel.SetActive(false);
        _pauseButton.SetActive(true);
        Time.timeScale = 1f;
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
