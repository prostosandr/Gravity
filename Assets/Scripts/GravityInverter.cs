using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Rotator))]
public class GravityInverter : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Rotator _rotator;
    private float _defaultGravity;

    private bool _isInverted;

    public bool IsInverted => _isInverted;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rotator = GetComponent<Rotator>();

        _defaultGravity = Mathf.Abs(_rigidbody.gravityScale);
    }

    public void ToggleGravity(float currentMoveDirection)
    {
        _isInverted = !_isInverted;

        _rigidbody.gravityScale = _isInverted ? -_defaultGravity : _defaultGravity;

        _rotator.TurnForward(currentMoveDirection, _isInverted);
    }
}