using Unity.Cinemachine;
using UnityEngine;

public class HideSpot : MonoBehaviour
{
    [Header("References")]
    [TagField]
    [SerializeField] private string _playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            HideMenuManager.Instance.ShowHideButton(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            HideMenuManager.Instance.HideAll();
        }
    }
}
