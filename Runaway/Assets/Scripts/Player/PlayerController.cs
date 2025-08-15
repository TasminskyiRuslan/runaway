using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Touch control settings")]
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private Transform cameraTransform;

    [Header("Player settings")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float smoothRotationTime = 0.05f;
    [SerializeField] private float jumpForce = 5.0f;

    [Header("Boost settings")]
    [SerializeField] private float boostSpeedMultiplier = 1.5f;
    [SerializeField] private float boostDuration = 3.0f;
    [SerializeField] private float boostCooldown = 5.0f;
    [SerializeField] private bool isBoostReady = true;

    private CharacterController _controller;
    private Vector3 _movementDirection;

    private float _currentRotationVelocity;
    private float _gravity = -9.81f;
    private float _gravityVelocity;
    private float _currentSpeed;
    private float _boostTimer;
    private float _cooldownTimer;

    private bool IsGrounded() => _controller.isGrounded;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _currentSpeed = movementSpeed;
    }

    private void Update()
    {
        MoveHandler();
        GravityHandler();
        ApplyRotation();
        ApplyMovement();
        UpdateBoostTimers();
    }

    private void UpdateBoostTimers()
    {
        // Boost active timer
        if (_boostTimer > 0)
        {
            _boostTimer -= Time.deltaTime;
            if (_boostTimer <= 0)
            {
                EndBoost();
            }
        }

        // Cooldown timer
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0)
            {
                isBoostReady = true;
            }
        }
    }

    private void MoveHandler()
    {
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
        Vector3 joystickDirection = new Vector3(joystick.Horizontal, 0.0f, joystick.Vertical);
        Vector3 cameraForward = new Vector3(cameraTransform.forward.x, 0.0f, cameraTransform.forward.z);
        Vector3 cameraRight = new Vector3(cameraTransform.right.x, 0.0f, cameraTransform.right.z);
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

    // Button OnClick() function for jump
    public void ApplyJump()
    {
        if (IsGrounded())
        {
            _gravityVelocity += jumpForce;
        }
    }

    // Button OnClick() function for boost
    public void ActivateBoost()
    {
        if (isBoostReady)
        {
            StartBoost();
        }
    }

    private void StartBoost()
    {
        _currentSpeed = movementSpeed * boostSpeedMultiplier;
        _boostTimer = boostDuration;
        isBoostReady = false;
        // Here you can add visual/audio effects for boost activation
    }

    private void EndBoost()
    {
        _currentSpeed = movementSpeed;
        _cooldownTimer = boostCooldown;
        // Here you can add visual/audio effects for boost deactivation
    }

    private void ApplyRotation()
    {
        if (_movementDirection.sqrMagnitude >= 0.1f)
        {
            var targetAngle = Mathf.Atan2(_movementDirection.x, _movementDirection.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentRotationVelocity, smoothRotationTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }
    }

    private void ApplyMovement()
    {
        _movementDirection.y = _gravityVelocity;
        _controller.Move(_movementDirection * _currentSpeed * Time.deltaTime);
    }
}