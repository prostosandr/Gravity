using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(Jumper))]
[RequireComponent(typeof(WallInteractor))]
[RequireComponent(typeof(AbilityCaster))]
[RequireComponent(typeof(Picker))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour, IDamageable
{
    [Header("Configurations")]
    [SerializeField] private PlayerConfig _config;
    [SerializeField] private PlayerInputProvider _input;
    [SerializeField] private WeaponHandler _weaponHandler;
    [SerializeField] private Gravity _gravity;
    [SerializeField] private GravityInverter _gravityInverter;

    [Header("Checkers")]
    [SerializeField] private ColliderChecker _groundChecker;
    [SerializeField] private ColliderChecker _wallChecker;

    private Mover _mover;
    private Jumper _jumper;
    private Rotator _rotator;
    private WallInteractor _wallInteractor;
    private AbilityCaster _caster;
    private Picker _picker;
    private Health _health;
    private Rigidbody2D _rigidbody;

    private bool _canInvertGravity;
    private bool _isZeroGravity => _gravity.CurrentDirection == GravityDirection.Zero;
    private bool _canChangeGravity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _mover = GetComponent<Mover>();
        _jumper = GetComponent<Jumper>();
        _rotator = GetComponent<Rotator>();
        _wallInteractor = GetComponent<WallInteractor>();
        _caster = GetComponent<AbilityCaster>();
        _picker = GetComponent<Picker>();
        _health = GetComponent<Health>();

        _canInvertGravity = true;
        _canChangeGravity = true;
    }

    private void OnEnable()
    {
        _input.JumpPressed += OnJumpHandle;
        _input.InvertGravityPressed += OnGravityInvertRequest;
        _input.AbilityPressed += OnStartAbilityHandler;
        _input.AbilityRelesed += OnCancelAbilityHandle;
        _input.ShootPressed += OnShootHandle;
        _input.Reloaded += OnReloadHandle;
        _input.InteractionPressed += OnInteractObjectHandle;
        _input.ThrowPressed += OnThrowHandle;

        _gravityInverter.GravityChanged += OnGravityChanged;
        _gravityInverter.GravityInvertCompleted += OnGravityInvertCompleted;

        _health.Died += OnDieHandle;
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        _weaponHandler.Rotate(_input.GetAimDirection(_weaponHandler.transform.position));
        _weaponHandler.GunUpdate();

        if (_isZeroGravity)
            return;

        if (_input.IsCastAbilityHeld)
            _caster.OnAimAbility(_input.GetAimDirection(transform.position), _input.RadiusChangeInput);

        Vector2 upAxis = GravityUtils.GetUpAxis(_gravity.CurrentDirection);
        float jumpIntent = Vector2.Dot(_input.MoveDirection, upAxis);
        bool isJumpDirectionPressed = jumpIntent > 0.5f;

    }

    private void FixedUpdate()
    {
        if (_isZeroGravity == false)
        {
            bool isGrounded = _groundChecker.CheckCollider();
            bool isTouchingWall = _wallChecker.CheckCollider();
            GravityDirection currentGravity = _gravity.CurrentDirection;

            _wallInteractor.HandleWallSlide(isTouchingWall, isGrounded, currentGravity);

            if (_wallInteractor.IsOnWall == false && _wallInteractor.IsWallJumping == false)
            {
                _mover.Move(_input.MoveDirection, currentGravity);
                _rotator.TurnForward(_input.GetAimDirection(transform.position), currentGravity);
            }

            _jumper.ModifyFall(_input.IsJumpHeld, _wallInteractor.IsOnWall, isGrounded, currentGravity);
        }
    }

    private void OnDisable()
    {
        _input.JumpPressed -= OnJumpHandle;
        _input.InvertGravityPressed -= OnGravityInvertRequest;
        _input.AbilityPressed -= OnStartAbilityHandler;
        _input.AbilityRelesed -= OnCancelAbilityHandle;
        _input.ShootPressed -= OnShootHandle;
        _input.Reloaded -= OnReloadHandle;
        _input.InteractionPressed -= OnInteractObjectHandle;
        _input.ThrowPressed -= OnThrowHandle;

        _gravityInverter.GravityChanged -= OnGravityChanged;
        _gravityInverter.GravityInvertCompleted -= OnGravityInvertCompleted;

        _health.Died -= OnDieHandle;
    }

    public void ActivateGravityInverter()
    {
        _canChangeGravity = true;
    }

    public void DeactivateGravityInverter()
    {
        _canChangeGravity = false;
    }

    public void SetGravityInversionStatus(bool canInvert)
    {
        _canInvertGravity = canInvert;
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _mover.enabled = isEnabled;
    }

    public void TakeDamage(float damage)
    {
        _health.TakeDamage(damage);
    }

    private void Initialize()
    {
        _mover.Initialize(_rigidbody, _config.Speed, _config.InterpolationSpeed);
        _jumper.Initialize(_rigidbody, _config.JumpForce, _config.FallForce, _config.JumpBrackingForce);
        _wallInteractor.Initialize(_rigidbody, _config.WallJumpForce, _config.WallSlideSpeed, _config.DelayAfterWalljump, _config.WallJumpVerticalBoost);
        _gravityInverter.Initialize(_gravity, _config.InvertCooldown);
        _weaponHandler.Initialize();
        _picker.Initialize(_config.ThrowPower);

        _rotator.AlignToGravity(GravityDirection.Down);

        _health.Initialize(_config.MaxHealth);
    }

    private void OnGravityInvertRequest()
    {
        if (_canInvertGravity == false || _canChangeGravity == false)
            return;

        GravityDirection target;

        if (Physics2D.gravity.y < 0)
            target = GravityDirection.Up;
        else
            target = GravityDirection.Down;

        _gravityInverter.ChangeGravity(target);

        _canInvertGravity = false;
    }

    private void OnGravityChanged(GravityDirection gravityDirection)
    {
        _rotator.AlignToGravity(gravityDirection);
        _weaponHandler.OnGravityChanged(gravityDirection);
    }

    private void OnGravityInvertCompleted()
    {
        _canInvertGravity = true;
    }

    private void OnJumpHandle()
    {
        if (_isZeroGravity)
            return;

        if (_wallInteractor.TryWallJump(_gravity.CurrentDirection))
        {
            _rotator.TurnForward(-transform.right, _gravity.CurrentDirection);
        }
        else
        {
            _jumper.Jump(_groundChecker.CheckCollider(), _gravity.CurrentDirection);
        }
    }

    private void OnStartAbilityHandler()
    {
        if (_isZeroGravity)
            return;

        _caster.OnStartAbility();
    }

    private void OnCancelAbilityHandle()
    {
        if (_isZeroGravity)
            return;

        _caster.OnCancelAbility();
    }

    private void OnShootHandle(bool canPlaceGravityWell)
    {
        if (canPlaceGravityWell)
            _caster.Cast(_input.IsCastAbilityHeld);
        else
            _weaponHandler.Shoot();
    }

    private void OnReloadHandle()
    {
        _weaponHandler.Reload();
    }

    private void OnInteractObjectHandle()
    {
        _picker.InteractObject();
    }

    private void OnThrowHandle()
    {
        _picker.Throw(_input.GetAimDirection(transform.position));
    }

    private void OnDieHandle()
    {
    }
}