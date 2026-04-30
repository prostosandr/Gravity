using UnityEngine;

public class GravityInverter : MonoBehaviour
{
    private bool _isInverted;

    public bool IsInverted => _isInverted;

    public void ToggleGravity(float currentMoveDirection)
    {
        _isInverted = !_isInverted;

        Physics2D.gravity = -Physics2D.gravity;
    }
}