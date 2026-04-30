using System.Collections;
using UnityEngine;

public class WallInteractor : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private float _wallJumpForce;
    private float _wallSlideSpeed;
    private float _delayAfterWallJump;
    private float _wallJumpVerticalBoost;

    private WaitForSeconds _wait;

    private bool _isOnWall;
    private bool _isWallJumping;

    public bool IsOnWall => _isOnWall;
    public bool IsWallJumping => _isWallJumping;

    public void Initialize(Rigidbody2D rigidbody, float wallJumpForce, float wallSlideSpeed, float delayAfterWallJump, float wallJumpVerticalBoost)
    {
        _rigidbody = rigidbody;
        _wallJumpForce = wallJumpForce;
        _wallSlideSpeed = wallSlideSpeed;
        _delayAfterWallJump = delayAfterWallJump;
        _wallJumpVerticalBoost = wallJumpVerticalBoost;

        _wait = new WaitForSeconds(_delayAfterWallJump);
    }

    public void HandleWallSlide(bool isInverted, bool touchingWall, bool touchingGround)
    {
        if (_isWallJumping)
            return;

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
                _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocityX, gravityDirection * _wallSlideSpeed);
            else
            {
                _isOnWall = false;
            }
        }
        else
        {
            _isOnWall = false;
        }
    }

    public bool TryWallJump(bool isInverted)
    {
        if (_isOnWall == false)
            return false;

        Vector2 directionAgainstWall = (Vector2)(-transform.right);

        Vector2 jumpDirection = ((Vector2)transform.up * _wallJumpVerticalBoost + directionAgainstWall).normalized;

        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.AddForce(jumpDirection * _wallJumpForce, ForceMode2D.Impulse);

        StartCoroutine(WallJumpRoutine());

        return true;
    }

    private IEnumerator WallJumpRoutine()
    {
        _isWallJumping = true;
        _isOnWall = false;

        yield return _wait;

        _isWallJumping = false;
    }
}