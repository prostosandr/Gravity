using UnityEngine;

[CreateAssetMenu(fileName = nameof(PlayerConfig), menuName = Constants.ConfigsMenus.PlayerMenuName + nameof(PlayerConfig))]
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

    [Header("GravityInverter")]
    [SerializeField] private float _invertCooldown;

    [Header("Picker")]
    [SerializeField] private float _throwPower;

    [Header("Health")]
    [SerializeField] private float _maxHealth;

    public float Speed => _speed;
    public float InterpolationSpeed => _interpolationSpeed;

    public float JumpForce => _jumpForce;
    public float FallForce => _fallForce;
    public float JumpBrackingForce => _jumpBrackingForce;

    public float WallJumpForce => _wallJumpForce;
    public float WallSlideSpeed => _wallSlideSpeed;
    public float DelayAfterWalljump => _delayAfterWallJump;
    public float WallJumpVerticalBoost => _wallJumpVerticalBoost;

    public float InvertCooldown => _invertCooldown;

    public float ThrowPower => _throwPower;

    public float MaxHealth => _maxHealth;
}
