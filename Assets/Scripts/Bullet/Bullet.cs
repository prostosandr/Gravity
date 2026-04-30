using System;
using UnityEngine;

[RequireComponent(typeof(BulletMover))]
[RequireComponent(typeof(BulletLifeCycle))]
public class Bullet : MonoBehaviour
{
    private BulletMover _mover;
    private BulletLifeCycle _lifeCycle;

    public event Action<Bullet> Deactivated;

    private void Awake()
    {
        _mover = GetComponent<BulletMover>();
        _lifeCycle = GetComponent<BulletLifeCycle>();
    }

    private void OnEnable()
    {
        _lifeCycle.Deactivated += Deactivate;
    }

    private void OnDisable()
    {
        _lifeCycle.Deactivated -= Deactivate;
    }

    public void Initialize(Vector3 direction)
    {
        _mover.SetDirection(direction);
        _lifeCycle.Initialize();
    }

    private void Deactivate()
    {
        _mover.Deactivate();

        Deactivated?.Invoke(this);
    }
}