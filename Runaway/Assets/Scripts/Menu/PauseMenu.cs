using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseButton;

    private void Start()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
        _pauseButton.SetActive(true);
    }
    public void Pause()
    {
        gameObject.SetActive(true);
        _pauseButton.SetActive(false);
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        _pauseButton.SetActive(true);
        Time.timeScale = 1.0f;
    }

    public void Menu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
