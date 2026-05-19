using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] private float _gravityScale;

    private GravityDirection _currentDirection;

    public GravityDirection CurrentDirection => _currentDirection;

    private void Awake()
    {
        _currentDirection = GravityDirection.Down;
    }

    public void SetGravityDirection(GravityDirection direction)
    {
        Physics2D.gravity = GravityUtils.GetVector(direction) * 9.81f;
        _currentDirection = direction;
    }
}