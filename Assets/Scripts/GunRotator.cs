using UnityEngine;

public class GunRotator : MonoBehaviour
{
    [SerializeField] private PlayerInputProvider _input;
    [SerializeField] private GravityInverter _gravityInverter;

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        Vector2 aimDirection;

        if (_input.IsAimingWithStick)
            aimDirection = _input.AimDirection;
        else
            aimDirection = _input.GetCursorPosition() - (Vector2)transform.position;

        if (aimDirection.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);

            Flip(angle);
        }
    }

    private void Flip(float angle)
    {
        if (angle > 90 || angle < -90)
            transform.localScale = new Vector2(1, -1);
        else
            transform.localScale = new Vector2(1, 1);

        if (_gravityInverter.IsInverted)
        {
            Vector2 newLocalScale = transform.localScale;

            newLocalScale.y = -newLocalScale.y;
            transform.localScale = newLocalScale;
        }
    }
}
