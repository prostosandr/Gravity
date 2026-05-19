using UnityEngine;

public class Rotator : MonoBehaviour
{
    private const float RightLook = 0;
    private const float LeftLook = 180;

    private float _currentGravityAngle;

    public void AlignToGravity(GravityDirection direction)
    {
        _currentGravityAngle = GravityUtils.GetRotationAngle(direction);
        ApplyRotation(transform.eulerAngles.y);
    }

    public void TurnForward(Vector2 aimDirection, GravityDirection gravityDirection)
    {
        Vector2 rightAxis = GravityUtils.GetRightAxis(gravityDirection);

        float forwardDot = Vector2.Dot(aimDirection, rightAxis);

        if (forwardDot >= 0)
            ApplyRotation(RightLook);
        else
            ApplyRotation(LeftLook);
    }

    private void ApplyRotation(float angleY)
    {
        Quaternion gravityRot = Quaternion.Euler(0, 0, _currentGravityAngle);
        Quaternion lookRot = Quaternion.Euler(0, angleY, 0);

        transform.rotation = gravityRot * lookRot;
    }
}