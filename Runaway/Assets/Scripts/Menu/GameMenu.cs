using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameMenuPanel;
    [SerializeField] private GameObject pauseButton;

    private void Start()
    {
        gameMenuPanel.SetActive(false);
        pauseButton.SetActive(true);
    }
    public void Pause()
    {
        gameMenuPanel.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        gameMenuPanel.SetActive(false);
        pauseButton.SetActive(true);
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
