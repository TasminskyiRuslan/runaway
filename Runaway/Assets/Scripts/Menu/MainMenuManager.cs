using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void OnPlayButtonClicked(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
