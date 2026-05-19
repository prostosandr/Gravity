using System;
using System.Collections;
using UnityEngine;

public class GravityInverter : MonoBehaviour
{
    private Gravity _gravity;

    private WaitForSeconds _wait;
    private Coroutine _coroutine;

    private float _cooldown;

    public GravityDirection CurrentDirection => _gravity.CurrentDirection;

    public event Action GravityInvertCompleted;
    public event Action<GravityDirection> GravityChanged;
    public event Action<float> CooldownStarted;

    public void Initialize(Gravity gravity,float cooldown)
    {
        _gravity = gravity;
        _cooldown = cooldown;

        _wait = new WaitForSeconds(_cooldown);
    }

    public void ChangeGravity(GravityDirection direction)
    {
        _gravity.SetGravityDirection(direction);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CooldownRoutine());

        CooldownStarted?.Invoke(_cooldown);
        GravityChanged?.Invoke(direction);
    }

    private IEnumerator CooldownRoutine()
    {
        yield return _wait;

        GravityInvertCompleted?.Invoke();
    }
}