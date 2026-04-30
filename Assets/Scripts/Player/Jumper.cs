using UnityEngine;

public class Jumper : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private float _jumpForce;
    private float _fallForce;
    private float _jumpBrakingForce;

    public void Initialize(Rigidbody2D rigidbody, float jumpForce, float fallForce, float jumoBrakingForce)
    {
        _rigidbody = rigidbody;
        _jumpForce = jumpForce;
        _fallForce = fallForce;
        _jumpBrakingForce = jumoBrakingForce;
    }

    public void TryJump(bool isGround)
    {
        if (isGround == false)
            return;

        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocityX, 0);
        _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);

    }

    public void ModifyFall(bool isJumpPressed, bool onWall, bool isGround, bool isGravityInverted)
    {
        if (onWall || isGround)
            return;

        bool isFalling = Mathf.Sign(_rigidbody.linearVelocityY) == Mathf.Sign(Physics2D.gravity.y);

        if (isFalling)
            Jump(_fallForce);
        else if (isFalling == false && isJumpPressed == false)
            Jump(_jumpBrakingForce);
    }

    private void Jump(float force)
    {
        float gravityDirection = Mathf.Sign(Physics2D.gravity.y);

        _rigidbody.linearVelocity += Vector2.up * gravityDirection * Mathf.Abs(Physics2D.gravity.y) * force * Time.fixedDeltaTime;
    }
}