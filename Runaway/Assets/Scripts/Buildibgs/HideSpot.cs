using Unity.Cinemachine;
using UnityEngine;

public class HideSpot : MonoBehaviour
{
    [Header("References")]
    public Transform exitPoint;
    [TagField]
    [SerializeField] private string _playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            if (PlayerInstance.Instance.Hide.IsHidden)
                HideMenuManager.Instance.ShowUnhideButton();
            else
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
