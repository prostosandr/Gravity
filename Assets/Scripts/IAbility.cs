using UnityEngine;

public interface IAbility
{
    public void Cast(bool isAbilityHeld);
    public void StartAbility();
    public void AimAbility(Vector2 aim, bool isGamepad, float radius);
    public void CancelAbility();
}