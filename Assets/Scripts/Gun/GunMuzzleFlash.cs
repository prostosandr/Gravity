using System.Collections;
using UnityEngine;

public class GunMuzzleFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _fire;

    private Coroutine _coroutine;
    private WaitForSeconds _wait;

    public void Initialize(float flashTime)
    {
        _wait = new WaitForSeconds(flashTime);
    }

    public void Flash()
    {
        if(_fire.gameObject.activeSelf)
            _fire.gameObject.SetActive(false);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        _fire.gameObject.SetActive(true);

        yield return _wait;

        _fire.gameObject.SetActive(false);
    }
}
