using UnityEngine;

public class Jumper : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private float _jumpForce;
    private float _fallForce;
    private float _jumpBrakingForce;

    public void Initialize(Rigidbody2D rigidbody, float jumpForce, float fallForce, float jumpBrakingForce)
    {
        _rigidbody = rigidbody;
        _jumpForce = jumpForce;
        _fallForce = fallForce;
        _jumpBrakingForce = jumpBrakingForce;
    }

    public void Jump(bool isGround, GravityDirection gravityDirection)
    {
        if (isGround == false)
            return;

        Vector2 rightAxis = GravityUtils.GetRightAxis(gravityDirection);
        Vector2 upAxis = GravityUtils.GetUpAxis(gravityDirection);

        float horizontalVelocity = Vector2.Dot(_rigidbody.linearVelocity, rightAxis);
        _rigidbody.linearVelocity = rightAxis * horizontalVelocity;

        _rigidbody.AddForce(upAxis * _jumpForce, ForceMode2D.Impulse);
    }

    public void ModifyFall(bool isJumpPressed, bool onWall, bool isGround, GravityDirection gravityDirection)
    {
        if (onWall || isGround)
            return;

        Vector2 upAxis = GravityUtils.GetUpAxis(gravityDirection);
        float verticalVelocity = Vector2.Dot(_rigidbody.linearVelocity, upAxis);

        if (verticalVelocity < 0)
            ApplyGravityBonus(_fallForce);
        else if (verticalVelocity > 0 && isJumpPressed == false)
            ApplyGravityBonus(_jumpBrakingForce);
    }

    private void ApplyGravityBonus(float multiplier)
    {
        _rigidbody.linearVelocity += Physics2D.gravity * multiplier * Time.fixedDeltaTime;
    }
}