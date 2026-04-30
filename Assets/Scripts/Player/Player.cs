using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(Jumper))]
[RequireComponent(typeof(WallInteractor))]
[RequireComponent(typeof(GravityInverter))]
[RequireComponent(typeof(AbilityCaster))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;
    [SerializeField] private PlayerInputProvider _input;

    [SerializeField] private ColliderChecker _groundChecker;
    [SerializeField] private ColliderChecker _wallChecker;

    [SerializeField] private Gun _gun;

    private Mover _mover;
    private Jumper _jumper;
    private Rotator _rotator;
    private WallInteractor _wallInteractor;
    private GravityInverter _gravityInverter;
    private AbilityCaster _caster;
    private PickUpLogic _pickUpLogic;

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
        _pickUpLogic = GetComponent<PickUpLogic>();
    }

    private void OnEnable()
    {
        Initialize();

        _input.JumpPressed += OnJumpHandle;
        _input.InvertGravityPressed += OnGravityHandle;
        _input.AbilityPressed += _caster.OnStartAbility;
        _input.AbilityRelesed += _caster.OnCancelAbility;
        _input.ShootPressed += OnShootHandle;
        _input.InteractionPressed += PickUpHandle;
    }

    private void OnDisable()
    {
        _input.JumpPressed -= OnJumpHandle;
        _input.InvertGravityPressed -= OnGravityHandle;
        _input.AbilityPressed -= _caster.OnStartAbility;
        _input.AbilityRelesed -= _caster.OnCancelAbility;
        _input.ShootPressed -= OnShootHandle;
        _input.InteractionPressed -= PickUpHandle;
    }

    private void Update()
    {
        if (_input.IsCastAbilityHeld)
        {
            if (_input.IsAimingWithStick)
                _caster.OnAimAbility(_input.AimDirection, true, _input.RadiusChangeInput);
            else
                _caster.OnAimAbility(_input.GetCursorPosition(), false, _input.RadiusChangeInput);
        }
    }

    private void FixedUpdate()
    {
        _wallInteractor.HandleWallSlide(_gravityInverter.IsInverted, _wallChecker.CheckCollider(), _groundChecker.CheckCollider());

        MoveHandle();

        _jumper.ModifyFall(_input.IsJumpHeld, _wallInteractor.IsOnWall, _groundChecker.CheckCollider(), _gravityInverter.IsInverted);
    }

    private void Initialize()
    {
        _mover.Initialize(_rigidbody, _config.Speed, _config.InterpolationSpeed);
        _jumper.Initialize(_rigidbody, _config.JumpForce, _config.FallForce, _config.JumpBrackingForce);
        _wallInteractor.Initialize(_rigidbody, _config.WallJumpForce, _config.WallSlideSpeed, _config.DelayAfterWalljump, _config.WallJumpVerticalBoost);
        _caster.Initialize();
    }

    private void MoveHandle()
    {
        if (_wallInteractor.IsOnWall == false && _wallInteractor.IsWallJumping == false)
        {
            if (Mathf.Abs(_input.MoveDirection.x) > 0.1f)
                _rotator.TurnForward(_input.MoveDirection.x, _gravityInverter.IsInverted);

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
            _rotator.TurnForward(-transform.right.x, _gravityInverter.IsInverted);
        }
    }

    private void OnGravityHandle()
    {
        _gravityInverter.ToggleGravity(_input.MoveDirection.x);
        _rotator.TurnForward(_input.MoveDirection.x, _gravityInverter.IsInverted);
    }

    private void OnShootHandle(bool canPlaceGravityWell)
    {
        if (canPlaceGravityWell)
            _caster.Cast(_input.IsCastAbilityHeld);
        else
            _gun.Shoot();
    }

    private void PickUpHandle()
    {
        _pickUpLogic.PickUpObject();
    }
}