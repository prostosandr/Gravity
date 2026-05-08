using UnityEngine;

public class AbilityCaster : MonoBehaviour
{
    [SerializeField] private GravityWellInteraction _ability;

    public void Cast(bool isAbilityHeld)
    {
        _ability.Cast(isAbilityHeld);
    }

    public void OnStartAbility()
    {
        _ability.StartAbility();
    }

    public void OnAimAbility(Vector2 directon, float radius)
    {
        _ability.AimAbility(directon, radius);
    }

    public void OnCancelAbility()
    {
        _ability.CancelAbility();
    }
}