using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Camera")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Animator animator;

    private CharacterController _controller;
    private Vector2 _moveInput;
    private Vector3 _velocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        ApplyGravity();
        UpdateAnimation();
        RotateToCamera();
    }

    private void Move()
    {
        // Camera-relative movement (your original idea)
        Vector3 move =
            _cameraTransform.forward * _moveInput.y +
            _cameraTransform.right * _moveInput.x;

        move.y = 0f; // prevent flying
        _controller.Move(move.normalized * _speed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _velocity.y < 0f)
            _velocity.y = -2f; // stick to ground

        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    // Input System callback (kept from your code)
    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    void RotateToCamera()
    {
        Vector3 forward = _cameraTransform.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(forward);
    }

    void UpdateAnimation()
    {
        animator.SetBool("IsMoving", _moveInput.sqrMagnitude > 0.01f);
    }
}
