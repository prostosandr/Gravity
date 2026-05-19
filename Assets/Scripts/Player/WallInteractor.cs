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

    public void HandleWallSlide(bool touchingWall, bool touchingGround, GravityDirection gravityDirection)
    {
        if (_isWallJumping)
            return;

        if (touchingWall && touchingGround == false)
        {
            _isOnWall = true;

            Vector2 rightAxis = GravityUtils.GetRightAxis(gravityDirection);
            Vector2 upAxis = GravityUtils.GetUpAxis(gravityDirection);

            float currentHoriz = Vector2.Dot(_rigidbody.linearVelocity, rightAxis);
            Vector2 slideVec = -upAxis * _wallSlideSpeed;

            _rigidbody.linearVelocity = rightAxis * currentHoriz + slideVec;
        }
        else
        {
            _isOnWall = false;
        }
    }

    public bool TryWallJump(GravityDirection gravityDirecition)
    {
        if (_isOnWall == false)
            return false;

        Vector2 upAxis = GravityUtils.GetUpAxis(gravityDirecition);
        Vector2 directionAgainstWall = (Vector2)(-transform.right);

        Vector2 jumpDirection = (upAxis * _wallJumpVerticalBoost + directionAgainstWall).normalized;

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