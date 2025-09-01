using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DynamicJoystick _joystick;
    [SerializeField] private Transform _cameraTransform;
    [TagField]
    [SerializeField] private string _joystickTag = "Joystick";
    [TagField]
    [SerializeField] private string _mainCameraTag = "MainCamera";

    [Header("Player settings")]
    [SerializeField] private float _movementSpeed = 5.0f;
    [SerializeField] private float _smoothRotationTime = 0.05f;
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private float _boostSpeedMultiplier = 1.5f;
    [SerializeField] private float _boostDuration = 3.0f;
    [SerializeField] private float _boostCooldown = 5.0f;
    [SerializeField] private bool _isBoostReady = true;

    // Movement fields
    private CharacterController _controller;
    private Vector3 _movementDirection;
    private bool _movementEnabled = true;

    // Gravity fields
    private float _currentRotationVelocity;
    private float _gravity = -9.81f;
    private float _gravityVelocity;

    // Boost fields
    private float _currentSpeed;
    private float _boostTimer;
    private float _cooldownTimer;

    private bool IsGrounded() => _controller.isGrounded;

    private void Awake()
    {
        _joystick = GameObject.FindGameObjectWithTag(_joystickTag)?.GetComponent<DynamicJoystick>();
        _cameraTransform = GameObject.FindGameObjectWithTag(_mainCameraTag)?.transform;
        _controller = GetComponent<CharacterController>();
        _currentSpeed = _movementSpeed;
    }

    private void Update()
    {
        MoveHandler();
        GravityHandler();
        ApplyRotation();
        ApplyMovement();
        UpdateBoostTimers();
    }

    public void SetMovementEnabled(bool enabled)
    {
        _movementEnabled = enabled;
    }

    private void MoveHandler()
    {
        if (!_movementEnabled) return;
        Vector3 currentDirection = GetCurrentDirecrion();
        if (currentDirection.magnitude >= 0.1f)
        {
            _movementDirection = currentDirection.normalized;
        }
        else
        {
            _movementDirection = Vector3.zero;
        }
    }

    private Vector3 GetCurrentDirecrion()
    {
        Vector3 joystickDirection = new Vector3(_joystick.Horizontal, 0.0f, _joystick.Vertical);
        Vector3 cameraForward = new Vector3(_cameraTransform.forward.x, 0.0f, _cameraTransform.forward.z);
        Vector3 cameraRight = new Vector3(_cameraTransform.right.x, 0.0f, _cameraTransform.right.z);
        Vector3 currentDirection = (cameraRight * joystickDirection.x + cameraForward * joystickDirection.z);
        return currentDirection;
    }

    private void GravityHandler()
    {
        if (IsGrounded() && _gravityVelocity < 0.0f)
        {
            _gravityVelocity = -1.0f;
        }
        else
        {
            _gravityVelocity += _gravity * Time.deltaTime;
        }
    }

    private void ApplyRotation()
    {
        if (!_movementEnabled) return;

        if (_movementDirection.sqrMagnitude >= 0.1f)
        {
            var targetAngle = Mathf.Atan2(_movementDirection.x, _movementDirection.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentRotationVelocity, _smoothRotationTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }
    }

    private void ApplyMovement()
    {
        if (!_movementEnabled) return;

        _movementDirection.y = _gravityVelocity;
        _controller.Move(_movementDirection * _currentSpeed * Time.deltaTime);
    }
    
    private void UpdateBoostTimers()
    {
        // Boost active timer
        if (_boostTimer > 0.0f)
        {
            _boostTimer -= Time.deltaTime;
            if (_boostTimer <= 0.0f)
            {
                EndBoost();
            }
        }

        // Cooldown timer
        if (_cooldownTimer > 0.0f)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0.0f)
            {
                _isBoostReady = true;
            }
        }
    }

    private void EndBoost()
    {
        _currentSpeed = _movementSpeed;
        _cooldownTimer = _boostCooldown;
    }

    // Button OnClick() function for boost
    public void ApplyBoost()
    {
        if (!_movementEnabled) return;

        if (_isBoostReady)
        {
            StartBoost();
        }
    }

    private void StartBoost()
    {
        _currentSpeed = _movementSpeed * _boostSpeedMultiplier;
        _boostTimer = _boostDuration;
        _isBoostReady = false;
    }

    // Button OnClick() function for jump
    public void ApplyJump()
    {
        if (!_movementEnabled) return;

        if (IsGrounded())
        {
            _gravityVelocity += _jumpForce;
        }
    }
}