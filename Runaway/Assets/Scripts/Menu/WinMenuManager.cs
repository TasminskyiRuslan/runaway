using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenuManager : MonoBehaviour
{
    public static WinMenuManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Button _leaveButton;
    [SerializeField] private GameObject _winMenuPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _leaveButton.gameObject.SetActive(false);
        _winMenuPanel.SetActive(false);
        _leaveButton.onClick.AddListener(OnLeaveButtonClicked);
    }

    public void ShowWinButton()
    {
        _leaveButton.gameObject.SetActive(true);
    }

    public void HideWinButton()
    {
        _leaveButton.gameObject.SetActive(false);
    }
    private void OnLeaveButtonClicked()
    {
        _winMenuPanel.SetActive(true);
        Time.timeScale = 0.0f;
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
