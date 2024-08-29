using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Touch control settings")]
    [SerializeField] private DynamicJoystick joystick;

    [Header("Player settings")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float smoothRotationTime = 0.05f;
    [SerializeField] private float jumpForce = 5.0f;

    private CharacterController _controller;
    private Vector3 _movementDirection;

    private float _currentRotationVelocity;
    private float _gravity = -9.81f;
    private float _gravityVelocity;

    private bool IsGrounded() => _controller.isGrounded;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MoveHandler();
        GravityHandler();
        ApplyRotation();
        ApplyMovement();
    }

    private void MoveHandler()
    {
        Vector3 inputDirection = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

        if (inputDirection.magnitude >= 0.1f)
        {
            _movementDirection = inputDirection.normalized;
        }
        else
        {
            _movementDirection = Vector3.zero;
        }
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

    // Button OnClick() function
    public void ApplyJump()
    {
        if (IsGrounded())
        {
            _gravityVelocity += jumpForce;
        }
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
        _controller.Move(_movementDirection * movementSpeed * Time.deltaTime);
    }
}