using UnityEngine;

[RequireComponent(typeof(GroundMovement))]
[RequireComponent(typeof(Jumper))]
[RequireComponent(typeof(WallInteractor))]
[RequireComponent(typeof(GravityInverter))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputProvider _input;

    private GroundMovement _movement;
    private Jumper _jumper;
    private WallInteractor _wallInteractor;
    private GravityInverter _gravityInverter;

    private void Awake()
    {
        _movement = GetComponent<GroundMovement>();
        _jumper = GetComponent<Jumper>();
        _wallInteractor = GetComponent<WallInteractor>();
        _gravityInverter = GetComponent<GravityInverter>();
    }

    private void OnEnable()
    {
        _input.JumpPressed += OnJumpHandle;
        _input.InvertGravityPressed += OnGravityHandle;
    }

    private void OnDisable()
    {
        _input.JumpPressed -= OnJumpHandle;
        _input.InvertGravityPressed -= OnGravityHandle;
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
}