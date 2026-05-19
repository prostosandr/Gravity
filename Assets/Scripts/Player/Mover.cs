using UnityEngine;

public class Mover : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private float _speed;
    private float _interpolationSpeed;

    public void Initialize(Rigidbody2D rigidbody, float speed, float interpolationSpeed)
    {
        _rigidbody = rigidbody;
        _speed = speed;
        _interpolationSpeed = interpolationSpeed;
    }

    public void Move(Vector2 moveDirection, GravityDirection gravityDirection)
    {
        Vector2 rightAxis = GravityUtils.GetRightAxis(gravityDirection);

        float desiredSpeed = Vector2.Dot(moveDirection, rightAxis) * _speed;

        float currentHorizontalVelocity = Vector2.Dot(_rigidbody.linearVelocity, rightAxis);
        float newHorizontalVelocity = Mathf.Lerp(currentHorizontalVelocity, desiredSpeed, Time.deltaTime * _interpolationSpeed);
        float velocityDelta = newHorizontalVelocity - currentHorizontalVelocity;

        _rigidbody.linearVelocity += rightAxis * velocityDelta;
    }
}