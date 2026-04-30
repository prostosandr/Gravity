using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputProvider : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private PlayerInput _input;

    public Vector2 MoveDirection => _input.Player.Move.ReadValue<Vector2>();
    public Vector2 AimDirection => _input.Player.Aim.ReadValue<Vector2>();

    public float RadiusChangeInput => _input.Player.ChangeRadius.ReadValue<float>();

    public bool IsJumpHeld => _input.Player.Jump.IsPressed();
    public bool IsCastAbilityHeld => _input.Player.CastAbility.IsPressed();
    public bool IsAimingWithStick => AimDirection.sqrMagnitude > 0.1f;

    public event Action JumpPressed;
    public event Action InvertGravityPressed;
    public event Action<bool> ShootPressed;
    public event Action AbilityPressed;
    public event Action AbilityRelesed;
    public event Action InteractionPressed;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();

        _input.Player.Jump.performed += ctx => JumpPressed?.Invoke();
        _input.Player.InvertGravity.performed += ctx => InvertGravityPressed?.Invoke();
        _input.Player.Shoot.performed += ctx => ShootPressed?.Invoke(IsCastAbilityHeld);
        _input.Player.Interaction.performed += ctx => InteractionPressed?.Invoke();
        _input.Player.CastAbility.performed += ctx => AbilityPressed?.Invoke();
        _input.Player.CastAbility.canceled += ctx => AbilityRelesed?.Invoke();
    }

    private void OnDisable()
    {
        _input.Player.Jump.performed -= ctx => JumpPressed?.Invoke();
        _input.Player.InvertGravity.performed -= ctx => InvertGravityPressed?.Invoke();
        _input.Player.Shoot.performed -= ctx => ShootPressed?.Invoke(IsCastAbilityHeld);
        _input.Player.Interaction.performed -= ctx => InteractionPressed?.Invoke();
        _input.Player.CastAbility.performed -= ctx => AbilityPressed?.Invoke();
        _input.Player.CastAbility.canceled -= ctx => AbilityRelesed?.Invoke();

        _input.Disable();
    }

    public Vector2 GetCursorPosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = -_camera.transform.position.z;

        return _camera.ScreenToWorldPoint(mousePosition);
    }
}
