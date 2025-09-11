using UnityEngine;
using UnityEngine.UI;

public class HideMenuManager : MonoBehaviour
{
    public static HideMenuManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Button _hideButton;
    [SerializeField] private Button _unhideButton;

    // Spot field
    private HideSpot _currentSpot;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _hideButton.gameObject.SetActive(false);
        _unhideButton.gameObject.SetActive(false);
        _hideButton.onClick.AddListener(OnHideButtonClicked);
        _unhideButton.onClick.AddListener(OnUnhideButtonClicked);
    }

    public void ShowHideButton(HideSpot spot)
    {
        _currentSpot = spot;
        _hideButton.gameObject.SetActive(true);
        _unhideButton.gameObject.SetActive(false);
    }

    public void ShowUnhideButton()
    {
        _hideButton.gameObject.SetActive(false);
        _unhideButton.gameObject.SetActive(true);
    }

    public void HideAll()
    {
        _hideButton.gameObject.SetActive(false);
        _unhideButton.gameObject.SetActive(false);
        _currentSpot = null;
    }

    private void OnHideButtonClicked()
    {
        if (_currentSpot != null)
        {
            PlayerInstance.Instance.Hide.Hide(_currentSpot);
            ShowUnhideButton();
        }
    }

    private void OnUnhideButtonClicked()
    {
        PlayerInstance.Instance.Hide.Unhide(_currentSpot);
        if (_currentSpot != null)
            ShowHideButton(_currentSpot);
    }
}
