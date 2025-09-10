using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenuManager : MonoBehaviour
{
    public static WinMenuManager Instance;

    [Header("References")]
    [SerializeField] private Button _leaveButton;
    [SerializeField] private GameObject _winMenuPanel;

    private void Awake()
    {
        Instance = this;
        _leaveButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        _winMenuPanel.SetActive(false);
        _leaveButton.onClick.AddListener(Leave);
    }

    public void ShowWinButton()
    {
        _leaveButton.gameObject.SetActive(true);
    }

    public void HideWinButton()
    {
        _leaveButton.gameObject.SetActive(false);
    }
    public void Leave()
    {
        _winMenuPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
