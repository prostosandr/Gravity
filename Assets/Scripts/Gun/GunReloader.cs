using System;
using System.Collections;
using UnityEngine;

public class GunReloader : MonoBehaviour
{
    private float _reloadTime;
    private WaitForSeconds _wait;
    private Coroutine _coroutine;
    private bool _isReload;

    public bool IsReload => _isReload;

    public event Action<float> ReloadStarted;
    public event Action ReloadCompleted;

    public void Initialize(float reloadTime)
    {
        _reloadTime = reloadTime;
        _wait = new WaitForSeconds(_reloadTime);
        _isReload = false;
    }

    public void Reload()
    {
        if (_isReload)
            return;

        _isReload = true;

        ReloadStarted?.Invoke(_reloadTime);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ReloadRoutine());
    }

    public IEnumerator ReloadRoutine()
    {
        yield return _wait;

        _isReload = false;

        ReloadCompleted?.Invoke();
    }
}
