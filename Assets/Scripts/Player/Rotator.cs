using UnityEngine;

public class Rotator : MonoBehaviour
{
    private const float RightLookAngle = 0;
    private const float LeftLookAngle = 180;
    private const float NormalGravityAngle = 0;
    private const float InvertedGravityAngle = 180;

    public void TurnForward(float direction, bool isInvertedGravity)
    {
        float angleZ = isInvertedGravity ? InvertedGravityAngle : NormalGravityAngle;
        float angleY = transform.eulerAngles.y;

        if (direction > 0)
            angleY = isInvertedGravity ? LeftLookAngle : RightLookAngle;
        else if (direction < 0)
            angleY = isInvertedGravity ? RightLookAngle : LeftLookAngle;

        transform.rotation = Quaternion.Euler(0, angleY, angleZ);
    }
}