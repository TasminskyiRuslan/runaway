using Unity.Cinemachine;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [Header("References")]
    [TagField]
    [SerializeField] private string _playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag) && PlayerClothing.Instance.IsFullyClothed)
        {
            WinMenuManager.Instance.ShowWinButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_playerTag) && PlayerClothing.Instance.IsFullyClothed)
        {
            WinMenuManager.Instance.HideWinButton();
        }
    }
}
