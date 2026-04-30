using System;
using System.Collections;
using UnityEngine;

public class BulletLifeCycle : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _damage;

    private WaitForSeconds _wait;
    private Coroutine _lifeCycle;

    public event Action Deactivated;

    private void Awake()
    {
        _wait = new WaitForSeconds(_lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;

        Deactivate();
    }

    public void Initialize()
    {
        _lifeCycle = StartCoroutine(StartLifeCycle());
    }

    public IEnumerator StartLifeCycle()
    {
        yield return _wait;

        Deactivate();
    }

    private void Deactivate()
    {
        if (_lifeCycle != null)
        {
            StopCoroutine(_lifeCycle);
            _lifeCycle = null;
        }

        Deactivated?.Invoke();
    }
}