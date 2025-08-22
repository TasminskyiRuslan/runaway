using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuActions : MonoBehaviour
{
    [SerializeField] private GameObject _pauseButton;

    private void Start()
    {
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
}
