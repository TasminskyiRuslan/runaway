using Unity.Cinemachine;
using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    public static PlayerHide Instance { get; private set; }
    public bool IsHidden { get; set; }

    [Header("References")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private CinemachineCamera _followingCamera;
    [TagField]
    [SerializeField] private string _followingCameraTag = "FollowingCamera";

    public HideSpot currentSpot;
    private CharacterController _characterController;
    private EnemyController _pullingEnemy;


    private void Awake()
    {
        Instance = this;
        _playerController = GetComponent<PlayerController>();
        _playerTransform = GetComponent<Transform>();
        _followingCamera = GameObject.FindGameObjectWithTag(_followingCameraTag)?.GetComponent<CinemachineCamera>();
        _characterController = GetComponent<CharacterController>();
        _playerAnimator = GetComponent<Animator>();
    }

    //public void Hide(HideSpot spot)
    //{
    //    IsHidden = true;

    //    foreach (var r in GetComponentsInChildren<Renderer>())
    //        r.enabled = false;

    //    if (_playerController != null)
    //        _playerController.SetMovementEnabled(false);

    //    if (_followingCamera != null)
    //        _followingCamera.LookAt = spot.transform;
    //}


    public void Hide(HideSpot spot)
    {
        IsHidden = true;

        _characterController.enabled = false;
        Vector3 hidePosition = new Vector3(spot.transform.position.x, _playerTransform.position.y, spot.transform.position.z);
        _playerTransform.position = hidePosition;
        _characterController.enabled = true;
        currentSpot = spot; // можна зберігати тут

        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        if (_playerController != null)
            _playerController.SetMovementEnabled(false);

        if (_followingCamera != null)
            _followingCamera.LookAt = spot.transform;
    }

    public void Unhide(HideSpot spot)
    {
        IsHidden = false;

        if (spot.exitPoint != null)
        {
            if (_characterController != null) _characterController.enabled = false;

            _playerTransform.position = spot.exitPoint.position;
            Vector3 dir = spot.exitPoint.position - spot.transform.position;
            dir.y = 0f;
            _playerTransform.rotation = Quaternion.LookRotation(dir != Vector3.zero ? dir : spot.transform.forward);

            if (_characterController != null) _characterController.enabled = true;
        }

        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = true;

        if (_playerController != null)
            _playerController.SetMovementEnabled(true);

        if (_followingCamera != null && _playerTransform != null)
            _followingCamera.LookAt = _playerTransform;
        currentSpot = null;

    }
}