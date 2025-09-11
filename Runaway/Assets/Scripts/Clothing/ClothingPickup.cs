using Unity.Cinemachine;
using UnityEngine;

public class ClothingPickup : MonoBehaviour
{
    public ClothingType type; // ← виправив на ClothingType, щоб було єдине

    [TagField]
    [SerializeField] private string _playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            ClothingMenuManager.Instance.ShowEquipButton(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            ClothingMenuManager.Instance.HideEquipButton();
        }
    }
}
