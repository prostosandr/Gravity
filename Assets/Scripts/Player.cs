using UnityEngine;

[RequireComponent(typeof(GroundMovement))]
[RequireComponent(typeof(Jumper))]
[RequireComponent(typeof(WallInteractor))]
[RequireComponent(typeof(GravityInverter))]
[RequireComponent(typeof(GravityWellInteraction))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputProvider _input;

    private GroundMovement _movement;
    private Jumper _jumper;
    private WallInteractor _wallInteractor;
    private GravityInverter _gravityInverter;
    private GravityWellInteraction _gravityWellInteraction;

    private void Awake()
    {
        _movement = GetComponent<GroundMovement>();
        _jumper = GetComponent<Jumper>();
        _wallInteractor = GetComponent<WallInteractor>();
        _gravityInverter = GetComponent<GravityInverter>();
        _gravityWellInteraction = GetComponent<GravityWellInteraction>();
    }

    private void OnEnable()
    {
        _input.JumpPressed += OnJumpHandle;
        _input.InvertGravityPressed += OnGravityHandle;
        _input.ShootPressed += OnGravityWellHandle;
    }

    private void OnDisable()
    {
        _input.JumpPressed -= OnJumpHandle;
        _input.InvertGravityPressed -= OnGravityHandle;
        _input.ShootPressed -= OnGravityWellHandle;
    }

    private void Update()
    {
        if (_input.IsPlaceGravityWellPressed)
        {
            if (_input.IsAimingWithStick)
                _gravityWellInteraction.UpdatePreview(_input.AimDirection, true);
            else
                _gravityWellInteraction.UpdatePreview(_input.GetCursorPosition(), false);
        }
        else
        {
            _gravityWellInteraction.DeactivatePreview();
            _gravityWellInteraction.DeactivateAimLine();
        }
    }

    private void FixedUpdate()
    {
        _wallInteractor.HandleWallSlide(_gravityInverter.IsInverted);

        bool canMove = _wallInteractor.IsOnWall == false && _wallInteractor.IsWallJumping == false;
        _movement.Move(_input.MoveDirection.x, canMove, _gravityInverter.IsInverted);

        _jumper.ModifyFall(_input.IsJumpPressed, _wallInteractor.IsOnWall, _gravityInverter.IsInverted);
    }

    private void OnJumpHandle()
    {
        if (!_wallInteractor.TryWallJump(_gravityInverter.IsInverted))
            _jumper.TryJump();
    }

    private void OnGravityHandle()
    {
        _gravityInverter.ToggleGravity(_input.MoveDirection.x);
    }

    private void OnGravityWellHandle(bool canPlaceGravityWell)
    {
        _gravityWellInteraction.PlaceGravityWell(canPlaceGravityWell);
    }
}