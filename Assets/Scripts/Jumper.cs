using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Jumper : MonoBehaviour
{
    [SerializeField] private ColliderChecker _groundChecker;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _fallForce;
    [SerializeField] private float _jumpBrakingForce;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void TryJump()
    {
        if (_groundChecker.CheckCollider())
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocityX, 0);
            _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    public void ModifyFall(bool isJumpPressed, bool onWall, bool isGravityInverted)
    {
        if (onWall || _groundChecker.CheckCollider()) 
            return;

        float gravityDirection = isGravityInverted ? 1f : -1f;

        bool isFalling = isGravityInverted ? _rigidbody.linearVelocityY > 0 : _rigidbody.linearVelocityY < 0;
        bool isAscending = isGravityInverted ? _rigidbody.linearVelocityY < 0 : _rigidbody.linearVelocityY > 0;

        if (isFalling && !isJumpPressed)
        {
            _rigidbody.linearVelocity += new Vector2(0, gravityDirection) * Mathf.Abs(Physics2D.gravity.y) * _fallForce * Time.fixedDeltaTime;
        }
        else if (isAscending && !isJumpPressed)
        {
            _rigidbody.linearVelocity += new Vector2(0, gravityDirection) * Mathf.Abs(Physics2D.gravity.y) * _jumpBrakingForce * Time.fixedDeltaTime;
        }
    }
}