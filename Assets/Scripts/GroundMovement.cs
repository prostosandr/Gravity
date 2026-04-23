using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Rotator))]
public class GroundMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _smoothFactor;

    private Rigidbody2D _rigidbody;
    private Rotator _rotator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rotator = GetComponent<Rotator>();
    }

    public void Move(float direction, bool canMove, bool isGravityInverted)
    {
        if (canMove == false)
            return;

        if (Mathf.Abs(direction) > 0.1f)
            _rotator.TurnForward(direction, isGravityInverted);

        float newVelocityX = Mathf.Lerp(_rigidbody.linearVelocityX, direction * _speed, Time.deltaTime * _smoothFactor);
        _rigidbody.linearVelocity = new Vector2(newVelocityX, _rigidbody.linearVelocityY);
    }
}