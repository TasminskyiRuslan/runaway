using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralActions : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void Menu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
