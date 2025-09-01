using Unity.Cinemachine;
using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    public static PlayerHide Instance { get; private set; }
    public bool IsHidden { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CinemachineCamera followingCamera;
    [TagField]
    [SerializeField] private string _followingCameraTag = "FollowingCamera";

    private void Awake()
    {
        Instance = this;
        playerController = GetComponent<PlayerController>();
        playerTransform = GetComponent<Transform>();
        followingCamera = GameObject.FindGameObjectWithTag(_followingCameraTag)?.GetComponent<CinemachineCamera>();
    }

    public void Hide(HideSpot spot)
    {
        IsHidden = true;
        
        if (playerController != null)
            playerController.SetMovementEnabled(false);

        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        if (followingCamera != null && spot != null)
        {
            followingCamera.LookAt = spot.transform;
        }
    }

    public void Unhide()
    {
        IsHidden = false;

        if (playerController != null)
            playerController.SetMovementEnabled(true);

        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = true;

        if (followingCamera != null && playerTransform != null)
        {
            followingCamera.LookAt = playerTransform;
        }
    }

}
