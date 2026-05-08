using UnityEngine;

public class WeaponHandlerRotator : MonoBehaviour
{
    public void Rotate(bool isGravityInverted, Vector2 aimDirection)
    {
        if (aimDirection.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);

            Flip(angle, isGravityInverted);
        }
    }

    private void Flip(float angle, bool isGravityInverted)
    {
        if (angle > 90 || angle < -90)
            transform.localScale = new Vector2(1, -1);
        else
            transform.localScale = new Vector2(1, 1);

        if (isGravityInverted)
        {
            Vector2 newLocalScale = transform.localScale;

            newLocalScale.y = -newLocalScale.y;
            transform.localScale = newLocalScale;
        }
    }
}
