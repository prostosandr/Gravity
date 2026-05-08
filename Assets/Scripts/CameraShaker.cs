using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private Gun _gun;
    [SerializeField] private float _shakeDuration;
    [SerializeField] private float _shakeMagnitude;

    private Vector3 _originalPosition;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _gun.Shooted += Shake;
    }

    private void OnDisable()
    {
        _gun.Shooted -= Shake;
    }

    public void Shake()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        _originalPosition = transform.localPosition;
        float elapsed = 0f;

        while(elapsed < _shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * _shakeMagnitude;
            float y = Random.Range(-1f, 1f) * _shakeMagnitude;

            transform.localPosition = new Vector3(_originalPosition.x + x, _originalPosition.y + y, _originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = _originalPosition;
    }
}
