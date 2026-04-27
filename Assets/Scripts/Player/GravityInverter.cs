using UnityEngine;

[RequireComponent(typeof(Rotator))]
public class GravityInverter : MonoBehaviour
{
    private Rotator _rotator;

    private bool _isInverted;

    public bool IsInverted => _isInverted;

    private void Awake()
    {
        _rotator = GetComponent<Rotator>();
    }

    public void ToggleGravity(float currentMoveDirection)
    {
        _isInverted = !_isInverted;

        Physics2D.gravity = -Physics2D.gravity;

        _rotator.TurnForward(currentMoveDirection, _isInverted);
    }
}