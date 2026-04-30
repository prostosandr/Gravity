using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BulletSpawner))]
public class Gun : MonoBehaviour
{
    [SerializeField] private Transform _recoilPivot;
    [SerializeField] private CameraShaker _shaker;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _reloadBar;
    [SerializeField] private float _magazineCapacity;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _fireTime;
    [SerializeField] private Transform _fire;

    [SerializeField] private float _recoilAngle;
    [SerializeField] private float _returnSpeed;

    private BulletSpawner _spawner;

    private WaitForSeconds _wait;

    private float _currentNumberOfbullets;
    private float _reloadTimer;

    private bool _canReload;

    private Coroutine _corutine;

    private Quaternion _initialRotation;

    private void Awake()
    {
        _spawner = GetComponent<BulletSpawner>();

        _canReload = false;
        _wait = new WaitForSeconds(_fireTime);

        _currentNumberOfbullets = _magazineCapacity;

        _initialRotation = _recoilPivot.localRotation;

        UpdateText();
    }

    private void Update()
    {
        if(_recoilPivot.localRotation != _initialRotation)
        {
            _recoilPivot.localRotation = Quaternion.Lerp(_recoilPivot.localRotation, _initialRotation, _returnSpeed * Time.deltaTime);
        }

        if(_canReload)
        {
            if(_reloadTimer < _reloadTime)
            {
                _reloadTimer += Time.deltaTime;

                _reloadBar.fillAmount = 0f + (_reloadTimer / _reloadTime);
            }
            else
            {
                _canReload = false;

                _currentNumberOfbullets = _magazineCapacity;

                _reloadBar.gameObject.SetActive(false);

                UpdateText();
            }
        }
    }

    public void Shoot()
    {
        if (_currentNumberOfbullets > 0)
        {
            _spawner.Spawn();
            _shaker.Shake();

            _recoilPivot.localRotation *= Quaternion.Euler(0, 0, _recoilAngle);

            _fire.gameObject.SetActive(true);

            if (_corutine != null)
                StopCoroutine(_corutine);

            _corutine = StartCoroutine(FireRoutine());

            _currentNumberOfbullets--;

            UpdateText();
        }
        else if(_canReload == false)
        {
            _reloadBar.gameObject.SetActive(true);

            _canReload = true;

            _reloadTimer = 0f;
        }
    }

    private void UpdateText()
    {
        _text.text = $"{_currentNumberOfbullets}/{_magazineCapacity}";
    }

    private IEnumerator FireRoutine()
    {
        yield return _wait;

        _fire.gameObject.SetActive(false);
    }
}
