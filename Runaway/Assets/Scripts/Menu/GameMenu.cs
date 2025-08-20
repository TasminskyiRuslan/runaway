using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject _gameMenuPanel;
    [SerializeField] private GameObject _pauseButton;

    private void Start()
    {
        _gameMenuPanel.SetActive(false);
        _pauseButton.SetActive(true);
    }
    public void Pause()
    {
        _gameMenuPanel.SetActive(true);
        _pauseButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        _gameMenuPanel.SetActive(false);
        _pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Menu(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
