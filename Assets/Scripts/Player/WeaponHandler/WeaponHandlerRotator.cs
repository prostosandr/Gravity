using UnityEngine;

public class WeaponHandlerRotator : MonoBehaviour
{
    public void Rotate(GravityDirection gravityDirection, Vector2 aimDirection)
    {
        if (aimDirection.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);

            Flip(gravityDirection, aimDirection);
        }
    }

    private void Flip(GravityDirection gravityDirection, Vector2 aimDirection)
    {
        Vector2 rightAxis = GravityUtils.GetRightAxis(gravityDirection);
        float forwardDot = Vector2.Dot(aimDirection, rightAxis);

        Vector3 scale = transform.localScale;

        if (forwardDot < 0)
        {
            scale.y = -1f;
        }
        else
        {
            scale.y = 1f;
        }

        transform.localScale = scale;
    }
}