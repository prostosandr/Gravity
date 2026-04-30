using UnityEngine;

[CreateAssetMenu(fileName = nameof(PlayerConfig), menuName = Constants.PlayerMenuName + nameof(PlayerConfig))]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float _speed;
    [SerializeField] private float _interpolationSpeed;

    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _fallForce;
    [SerializeField] private float _jumpBrackingForce;

    [Header("WallJump")]
    [SerializeField] private float _wallJumpForce;
    [SerializeField] private float _wallSlideSpeed;
    [SerializeField] private float _delayAfterWallJump;
    [SerializeField] private float _wallJumpVerticalBoost;

    public float Speed => _speed;
    public float InterpolationSpeed => _interpolationSpeed;

    public float JumpForce => _jumpForce;
    public float FallForce => _fallForce;
    public float JumpBrackingForce => _jumpBrackingForce;

    public float WallJumpForce => _wallJumpForce;
    public float WallSlideSpeed => _wallSlideSpeed;
    public float DelayAfterWalljump => _delayAfterWallJump;
    public float WallJumpVerticalBoost => _wallJumpVerticalBoost;
}
