using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GravityInverter : MonoBehaviour
{
    private WaitForSeconds _wait;
    private Coroutine _coroutine;

    private float _cooldown;

    private bool _isInverted;
    private bool _canInvert;

    public bool IsInverted => _isInverted;

    public event Action<bool> GravityInverted;
    public event Action<float> CooldownStarted;

    public void Initialize(float cooldown)
    {
        _cooldown = cooldown;

        _wait = new WaitForSeconds(_cooldown);

        _canInvert = true;
        _isInverted = false;
    }

    public void ToggleGravity()
    {
        if (_canInvert == false)
            return;

        _canInvert = false;

        _isInverted = !_isInverted;

        Physics2D.gravity = -Physics2D.gravity;

        GravityInverted?.Invoke(_isInverted);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CooldownRoutine());
        CooldownStarted?.Invoke(_cooldown);
    }

    private IEnumerator CooldownRoutine()
    {
        yield return _wait;

        _canInvert = true;
    }
}