using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInputProvider : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private PlayerInput _input;

    public Vector2 MoveDirection => _input.Player.Move.ReadValue<Vector2>();
    public Vector2 AimDirection => _input.Player.GravityWellAim.ReadValue<Vector2>();

    public float RadiusChangeInput => _input.Player.ChangeRadius.ReadValue<float>();

    public bool IsJumpPressed => _input.Player.Jump.IsPressed();
    public bool IsPlaceGravityWellPressed => _input.Player.PlaceGravityWell.IsPressed();
    public bool IsAimingWithStick => AimDirection.sqrMagnitude > 0.1f;

    public event Action JumpPressed;
    public event Action InvertGravityPressed;
    public event Action<bool> ShootPressed;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();

        _input.Player.Jump.performed += ctx => JumpPressed?.Invoke();
        _input.Player.InvertGravity.performed += ctx => InvertGravityPressed?.Invoke();
        _input.Player.Shoot.performed += ctx => ShootPressed?.Invoke(IsPlaceGravityWellPressed);
    }

    private void OnDisable()
    {
        _input.Player.Jump.performed -= ctx => JumpPressed?.Invoke();
        _input.Player.InvertGravity.performed -= ctx => InvertGravityPressed?.Invoke();
        _input.Player.Shoot.performed -= ctx => ShootPressed?.Invoke(IsPlaceGravityWellPressed);

        _input.Disable();
    }

    public Vector2 GetCursorPosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = -_camera.transform.position.z;

        return _camera.ScreenToWorldPoint(mousePosition);
    }
}
