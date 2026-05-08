using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ReloadBar : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    private Image _bar;

    private Coroutine _coroutine;

    public void Initialize()
    {
        _bar = GetComponent<Image>();
    }

    public void Update()
    {
        if (gameObject.activeSelf)
            transform.position = _target.position + _offset;
    }

    public void StartLoading(float reloadTime)
    {
        if (_bar.gameObject.activeSelf)
            _bar.gameObject.SetActive(false);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _bar.gameObject.SetActive(true);

        _coroutine = StartCoroutine(FillRoutine(reloadTime));
    }

    private IEnumerator FillRoutine(float reloadTime)
    {
        float currentReloadTime = 0f;

        while (currentReloadTime < reloadTime)
        {
            currentReloadTime += Time.deltaTime;

            _bar.fillAmount = currentReloadTime / reloadTime;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
