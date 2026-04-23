using System;
using UnityEngine;

public class PlayerInputProvider : MonoBehaviour
{
    private PlayerInput _input;

    public Vector2 MoveDirection => _input.Player.Move.ReadValue<Vector2>();

    public bool IsJumpPressed => _input.Player.Jump.IsPressed();

    public event Action JumpPressed;
    public event Action InvertGravityPressed;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();

        _input.Player.Jump.performed += ctx => JumpPressed?.Invoke();
        _input.Player.InvertGravity.performed += ctx => InvertGravityPressed?.Invoke();
    }

    private void OnDisable()
    {
        _input.Player.Jump.performed -= ctx => JumpPressed?.Invoke();
        _input.Player.InvertGravity.performed -= ctx => InvertGravityPressed?.Invoke();

        _input.Disable();
    }
}
