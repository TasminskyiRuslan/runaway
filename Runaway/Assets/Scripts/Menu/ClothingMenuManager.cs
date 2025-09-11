using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothingMenuManager : MonoBehaviour
{
    public static ClothingMenuManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Button _equipButton;

    // Item field
    private ClothingPickup _currentItem;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _equipButton.gameObject.SetActive(false);
        _equipButton.onClick.AddListener(OnEquipButtonClicked);
    }

    public void ShowEquipButton(ClothingPickup item)
    {
        _currentItem = item;
        _equipButton.gameObject.SetActive(true);
        _equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equip " + item.type.ToString();
    }

    public void HideEquipButton()
    {
        _currentItem = null;
        _equipButton.gameObject.SetActive(false);
    }

    private void OnEquipButtonClicked()
    {
        if (_currentItem != null)
        {
            PlayerInstance.Instance.Clothing.Equip(_currentItem.type);
            Destroy(_currentItem.gameObject);
            HideEquipButton();
        }
    }
}
