using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class BossTarget : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private ParticleSystem _expode;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();

        _health.Initialize(_maxHealth);
    }

    private void OnEnable()
    {
        _health.Died += Explode;
    }

    private void OnDisable()
    {
        _health.Died -= Explode;
    }

    private void Explode()
    {
        _expode.transform.SetParent(null);
        _expode.Play();
        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        _health.TakeDamage(damage);
    }
}
