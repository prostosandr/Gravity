using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction)
    {
        _rigidbody.linearVelocity = direction * _speed;
    }

    public void Deactivate()
    {
        _rigidbody.linearVelocity = Vector2.zero;
    }
}