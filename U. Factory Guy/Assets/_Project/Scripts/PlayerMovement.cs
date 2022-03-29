using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _input;
    private Rigidbody _rb;

    private InputAction _move;

    private Transform _transform;
    private Vector2 _moveInput;
    private Vector3 _moveDirection;

    [SerializeField] private float _moveSpeed;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();

        _move = _input.actions["Move"];
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        _moveInput = _move.ReadValue<Vector2>();
        if (_moveInput == Vector2.zero) return;

        _moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
        //_transform.LookAt(_rb.velocity.normalized);
        _rb.velocity = _moveDirection * _moveSpeed * Time.fixedDeltaTime;
    }
}
