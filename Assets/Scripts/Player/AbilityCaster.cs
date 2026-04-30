using UnityEngine;

public class AbilityCaster : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _ability;

    IAbility _currentAbility;

    public void Initialize()
    {
        _currentAbility = _ability as IAbility;
    }

    public void Cast(bool isAbilityHeld)
    {
        _currentAbility.Cast(isAbilityHeld);
    }

    public void OnStartAbility()
    {
        _currentAbility.StartAbility();
    }

    public void OnAimAbility(Vector2 aim, bool isGamepad, float radius)
    {
        _currentAbility.AimAbility(aim, isGamepad, radius);
    }

    public void OnCancelAbility()
    {
        _currentAbility.CancelAbility();
    }
}
