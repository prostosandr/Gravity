using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(Jumper))]
[RequireComponent(typeof(WallInteractor))]
[RequireComponent(typeof(GravityInverter))]
[RequireComponent(typeof(AbilityCaster))]
[RequireComponent(typeof(Picker))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;
    [SerializeField] private PlayerInputProvider _input;
    [SerializeField] private WeaponHandler _weaponHandler;

    [SerializeField] private ColliderChecker _groundChecker;
    [SerializeField] private ColliderChecker _wallChecker;

    private Mover _mover;
    private Jumper _jumper;
    private Rotator _rotator;
    private WallInteractor _wallInteractor;
    private GravityInverter _gravityInverter;
    private AbilityCaster _caster;
    private Picker _picker;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _mover = GetComponent<Mover>();
        _jumper = GetComponent<Jumper>();
        _rotator = GetComponent<Rotator>();
        _wallInteractor = GetComponent<WallInteractor>();
        _gravityInverter = GetComponent<GravityInverter>();
        _caster = GetComponent<AbilityCaster>();
        _picker = GetComponent<Picker>();
    }

    private void OnEnable()
    {
        Initialize();

        _input.JumpPressed += OnJumpHandle;
        _input.InvertGravityPressed += OnGravityHandle;
        _input.AbilityPressed += OnAbilityStartHandle;
        _input.AbilityRelesed += OnAbilityCancelHandle;
        _input.ShootPressed += OnShootHandle;
        _input.Reloaded += OnReloadHandle;
        _input.InteractionPressed += PickUpHandle;
        _input.ThrowPressed += ThrowHandle;

        _gravityInverter.GravityInverted += _weaponHandler.OnGravityInvert;
    }

    private void Update()
    {
        _weaponHandler.Rotate();
        _weaponHandler.GunUpdate(); 

        if (_input.IsCastAbilityHeld)
        {
            _caster.OnAimAbility(_input.GetAimDirection(transform.position), _input.RadiusChangeInput);
        }
    }

    private void FixedUpdate()
    {
        _wallInteractor.HandleWallSlide(_gravityInverter.IsInverted, _wallChecker.CheckCollider(), _groundChecker.CheckCollider());

        MoveHandle();

        _jumper.ModifyFall(_input.IsJumpHeld, _wallInteractor.IsOnWall, _groundChecker.CheckCollider(), _gravityInverter.IsInverted);

        if (_wallInteractor.IsOnWall == false && _wallInteractor.IsWallJumping == false)
            _rotator.TurnForward(_input.GetAimDirection(transform.position), _gravityInverter.IsInverted);
    }

    private void OnDisable()
    {
        _input.JumpPressed -= OnJumpHandle;
        _input.InvertGravityPressed -= OnGravityHandle;
        _input.AbilityPressed -= OnAbilityStartHandle;
        _input.AbilityRelesed -= OnAbilityCancelHandle;
        _input.ShootPressed -= OnShootHandle;
        _input.Reloaded -= OnReloadHandle;
        _input.InteractionPressed -= PickUpHandle;
        _input.ThrowPressed -= ThrowHandle;

        _gravityInverter.GravityInverted -= _weaponHandler.OnGravityInvert;
    }

    private void Initialize()
    {
        _mover.Initialize(_rigidbody, _config.Speed, _config.InterpolationSpeed);
        _jumper.Initialize(_rigidbody, _config.JumpForce, _config.FallForce, _config.JumpBrackingForce);
        _wallInteractor.Initialize(_rigidbody, _config.WallJumpForce, _config.WallSlideSpeed, _config.DelayAfterWalljump, _config.WallJumpVerticalBoost);
        _gravityInverter.Initialize(_config.InvertCooldown);
        _weaponHandler.Initialize();
        _picker.Initialize(_config.ThrowPower);
    }

    private void MoveHandle()
    {
        if (_wallInteractor.IsOnWall == false && _wallInteractor.IsWallJumping == false)
        {
            if (Mathf.Abs(_input.MoveDirection.x) > 0.1f)
                _rotator.TurnForward(_input.GetAimDirection(transform.position), _gravityInverter.IsInverted);

            _mover.Move(_input.MoveDirection.x);
        }
    }

    private void OnJumpHandle()
    {
        if (_wallInteractor.TryWallJump(_gravityInverter.IsInverted) == false)
        {
            _jumper.TryJump(_groundChecker.CheckCollider());
        }
        else
        {
            _rotator.TurnForward(-transform.right, _gravityInverter.IsInverted);
        }
    }

    private void OnGravityHandle()
    {
        _gravityInverter.ToggleGravity();
        _rotator.TurnForward(_input.GetAimDirection(transform.position), _gravityInverter.IsInverted);
    }

    private void OnShootHandle(bool canPlaceGravityWell)
    {
        if (canPlaceGravityWell)
            _caster.Cast(_input.IsCastAbilityHeld);
        else
            _weaponHandler.Shoot();
    }

    private void OnAbilityStartHandle()
    {
        _caster.OnStartAbility();
    }

    private void OnAbilityCancelHandle()
    {
        _caster.OnCancelAbility();
    }

    private void OnReloadHandle()
    {
        _weaponHandler.Reload();
    }

    private void PickUpHandle()
    {
        _picker.InteractObject();
    }

    private void ThrowHandle()
    {
        _picker.Throw(_input.GetAimDirection(transform.position));
    }
}