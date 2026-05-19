using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _blood;

    private float _maxHealth;
    private float _currentHealth;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;

    public event Action<float, float> HealthChanged;
    public event Action Died;

    public void Initialize(float maxHealth)
    {
        _maxHealth = maxHealth;

        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
            _currentHealth = 0;

        HealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth == 0)
            Die();
    }

    public void Die()
    {
        if (_blood != null)
        {
            _blood.transform.SetParent(null);
            _blood.gameObject.SetActive(true);
        }

        Died?.Invoke();

        gameObject.SetActive(false);
    }
}