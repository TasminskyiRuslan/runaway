using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeScaleSetter : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1.0f; // завжди скидаємо час при завантаженні сцени
    }
}
