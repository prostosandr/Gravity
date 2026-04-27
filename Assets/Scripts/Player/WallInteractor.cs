using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Rotator))]
public class WallInteractor : MonoBehaviour
{
    [SerializeField] private ColliderChecker _groundChecker;
    [SerializeField] private ColliderChecker _wallChecker;

    [SerializeField] private float _wallJumpForce;
    [SerializeField] private float _wallSlideSpeed;
    [SerializeField] private float _delayAfterWallJump;
    [SerializeField] private float _wallJumpVerticalBoost;

    private Rigidbody2D _rigidbody;
    private Rotator _rotator;

    private bool _isOnWall;
    private bool _isWallJumping;

    public bool IsOnWall => _isOnWall;
    public bool IsWallJumping => _isWallJumping;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rotator = GetComponent<Rotator>();
    }

    public void HandleWallSlide(bool isInverted)
    {
        if (_isWallJumping)
            return;

        bool touchingWall = _wallChecker.CheckCollider();
        bool touchingGround = _groundChecker.CheckCollider();

        if (touchingWall && touchingGround == false)
        {
            if (_isOnWall == false)
            {
                _rigidbody.linearVelocity = Vector2.zero;
                _isOnWall = true;
            }

            float gravityDirection = Mathf.Sign(Physics2D.gravity.y);

            bool isMovingWithGravity = Mathf.Sign(_rigidbody.linearVelocityY) == gravityDirection;

            if (isMovingWithGravity || Mathf.Abs(_rigidbody.linearVelocityY) < 0.1f)
            {
                _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocityX, gravityDirection * _wallSlideSpeed);
            }
            else
            {
                _isOnWall = false;
            }
        }
    }

    public bool TryWallJump(bool isInverted)
    {
        if (_isOnWall == false)
            return false;

        Vector2 awayFromWall = (Vector2)(-transform.right);

        Vector2 jumpDirection = ((Vector2)transform.up * _wallJumpVerticalBoost + awayFromWall).normalized;

        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.AddForce(jumpDirection * _wallJumpForce, ForceMode2D.Impulse);

        _rotator.TurnForward(awayFromWall.x, isInverted);

        StartCoroutine(WallJumpRoutine());

        return true;
    }

    private IEnumerator WallJumpRoutine()
    {
        _isWallJumping = true;
        _isOnWall = false;

        yield return new WaitForSeconds(_delayAfterWallJump);

        _isWallJumping = false;
    }
}