using UnityEngine;

public class GravityTest : MonoBehaviour
{
    [SerializeField] private GravityInverter _inverter;
    [SerializeField] private PlayerInputProvider _input;

    private void OnEnable()
    {
        _input.RightGravityPressed += SetGravityRight;
        _input.LeftGravityPressed += SetGravityLeft;
        _input.ZeroGravityPressed += SetZeroGravity;
    }

    private void SetGravityRight()
    {
        _inverter.ChangeGravity(GravityDirection.Right);
    }

    private void SetGravityLeft()
    { 
        _inverter.ChangeGravity(GravityDirection.Left);
    }

    private void SetZeroGravity()
    {
        _inverter.ChangeGravity(GravityDirection.Zero);
    }
}