using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    public void Play(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
