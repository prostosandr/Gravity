using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private HealthBar _bar;
    [SerializeField] private Health _health;

    private void OnEnable()
    {
        _health.HealthChanged += _bar.UpdateBar;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= _bar.UpdateBar;
    }
}
