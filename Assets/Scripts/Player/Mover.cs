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

    public void Move(float direction)
    {
        float newVelocityX = Mathf.Lerp(_rigidbody.linearVelocityX, direction * _speed, Time.deltaTime * _interpolationSpeed);
        _rigidbody.linearVelocity = new Vector2(newVelocityX, _rigidbody.linearVelocityY);
    }
}