using UnityEngine;

public static class GravityUtils
{
    public static Vector2 GetVector(GravityDirection direction)
    {
        switch (direction)
        {
            case GravityDirection.Down:
                return Vector2.down;

            case GravityDirection.Left:
                return Vector2.left;

            case GravityDirection.Up:
                return Vector2.up;

            case GravityDirection.Right:
                return Vector2.right;

            case GravityDirection.Zero:
                return Vector2.zero;

            default:
                return Vector2.down;
        }
    }

    public static Vector2 GetUpAxis(GravityDirection direction)
    {
        if (direction == GravityDirection.Zero)
            return Vector2.up;
        else
            return -GetVector(direction);
    }

    public static Vector2 GetRightAxis(GravityDirection direction)
    {
        Vector2 up = GetUpAxis(direction);
        return new Vector2(up.y, -up.x);
    }

    public static float GetRotationAngle(GravityDirection direction)
    {
        switch (direction)
        {
            case GravityDirection.Down:
                return Constants.GravitationalParameters.AngleDown;

            case GravityDirection.Left:
                return Constants.GravitationalParameters.AngleLeft;

            case GravityDirection.Up:
                return Constants.GravitationalParameters.AngleUp;

            case GravityDirection.Right:
                return Constants.GravitationalParameters.AngleRight;

            case GravityDirection.Zero:
                return Constants.GravitationalParameters.AngleDown;

            default:
                return Constants.GravitationalParameters.AngleDown;
        }
    }
}