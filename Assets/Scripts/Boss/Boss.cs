using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private CameraSetter _camera;
    [SerializeField] private float _bossCameraFieldOfView;

    [Header("Phase One")]
    [SerializeField] private BossTrigger _bossTrigger;
    [SerializeField] private Player _player;
    [SerializeField] private List<BossContainer> _containers;
    [SerializeField] private List<Transform> _platforms;
    [SerializeField] private float _containerReturnSpeed;
    [SerializeField] private float _throwContainerPower;
    [SerializeField] private float _aimingTime;
    [SerializeField] private float _containerThrowDelay;
    [SerializeField] private Health _phaseOneHeatlh;
    [SerializeField] private HealthUI _phaseOneHeatlhUI;

    [Header("Phase Two")]
    [SerializeField] private GravityInverter _inverter;
    [SerializeField] private List<Spike> _spikes;
    [SerializeField] private float _delayPerChangeGravity;
    [SerializeField] private List<GravityArrow> _gravityArrows;
    [SerializeField] private Health _phaseTwoHeatlh;
    [SerializeField] private HealthUI _phaseTwoHeatlhUI;
    [SerializeField] private float _flickTime;

    [Header("Phase Three")]
    [SerializeField] private LineRenderer _aimLine;
    [SerializeField] private Transform _laserStart;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private LayerMask _hitLayers;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _damage;
    [SerializeField] private float _damageInterval;
    [SerializeField] private Health _phaseThreeHeatlh;
    [SerializeField] private HealthUI _phaseThreeHeatlhUI;


    [SerializeField] private ParticleSystem _explode;

    private float _damageTimer;

    private GravityDirection[] _gravityDirection;
    private bool _canChangeGravity;

    private BossStates _state;

    private BossContainer _currentContainer;

    private bool _canThrowContainer;
    private bool _isContainerMoveToBoss;
    private bool _isAiming;
    private bool _isThrowContainerDelay;

    private void Awake()
    {
        _state = BossStates.Idle;

        _canThrowContainer = false;
        _isAiming = false;
        _isThrowContainerDelay = false;
        _isContainerMoveToBoss = false;

        _gravityDirection = (GravityDirection[])System.Enum.GetValues(typeof(GravityDirection));
        _canChangeGravity = true;
    }

    private void OnEnable()
    {
        _bossTrigger.OnTriggerEntered += Activate;
        _phaseOneHeatlh.Died += SetPhaseTwo;
        _phaseTwoHeatlh.Died += SetPhaseThree;
        _phaseThreeHeatlh.Died += Die;
    }

    private void OnDisable()
    {
        _bossTrigger.OnTriggerEntered -= Activate;
        _phaseOneHeatlh.Died -= SetPhaseTwo;
        _phaseTwoHeatlh.Died -= SetPhaseThree;
        _phaseThreeHeatlh.Died -= Die;
    }

    private void Update()
    {
        switch (_state)
        {
            case BossStates.Idle:
                return;

            case BossStates.PhaseOne:
                PhaseOne();
                break;

            case BossStates.PhaseTwo:
                PhaseTwo();
                break;

            case BossStates.PhaseThree:
                PhaseThree();
                break;

            default:
                return;
        }
    }

    private void Activate()
    {
        _state = BossStates.PhaseOne;

        _camera.SetFieldOfView(_bossCameraFieldOfView);
    }

    private void SetPhaseTwo()
    {
        foreach (var container in _containers)
        {
            container.gameObject.SetActive(false);
        }

        foreach (var platform in _platforms)
        {
            platform.gameObject.SetActive(false);
        }

        foreach (var spike in _spikes)
        {
            spike.gameObject.SetActive(true);
        }

        _phaseOneHeatlhUI.gameObject.SetActive(false);
        _player.DeactivateGravityInverter();

        _state = BossStates.PhaseTwo;

        _phaseTwoHeatlh.gameObject.SetActive(true);
        _phaseTwoHeatlhUI.gameObject.SetActive(true);
    }

    private void SetPhaseThree()
    {
        StopAllCoroutines();

        _phaseTwoHeatlh.gameObject.SetActive(false);
        _phaseTwoHeatlhUI.gameObject.SetActive(false);

        _inverter.ChangeGravity(GravityDirection.Down);

        foreach (var arrow in _gravityArrows)
        {
            arrow.gameObject.SetActive(false);
        }

        foreach (var spike in _spikes)
        {
            spike.gameObject.SetActive(false);
        }

        _inverter.ChangeGravity(GravityDirection.Zero);

        _state = BossStates.PhaseThree;
        _inverter.ChangeGravity(GravityDirection.Zero);
        _phaseThreeHeatlh.gameObject.SetActive(true);
        _phaseThreeHeatlhUI.gameObject.SetActive(true);
    }

    private void Die()
    {
        _explode.transform.SetParent(null);
        _explode.Play();
        gameObject.SetActive(false);
    }

    private void PhaseOne()
    {
        if (_currentContainer == null && _isThrowContainerDelay == false)
        {
            _currentContainer = _containers[Random.Range(0, _containers.Count)];
            _isContainerMoveToBoss = true;
            _currentContainer.SetKinematic();
        }

        if (_isThrowContainerDelay == false)
        {
            if (_isContainerMoveToBoss)
            {
                _currentContainer.transform.position = Vector2.MoveTowards(_currentContainer.transform.position, transform.position, _containerReturnSpeed * Time.deltaTime);

                if (_currentContainer.transform.position == transform.position)
                    _isContainerMoveToBoss = false;
            }
            else
            {
                if (_isAiming == false)
                    StartCoroutine(AimRoutine());
            }
        }

        if (_canThrowContainer && _currentContainer != null)
        {
            _currentContainer.SetDynamic();
            _currentContainer.ActivateAttack();

            Vector2 direction = _player.transform.position - transform.position;
            direction = direction.normalized;

            _currentContainer.Rigidbody.AddForce(direction * _throwContainerPower, ForceMode2D.Impulse);

            _canThrowContainer = false;

            if (_isThrowContainerDelay == false)
                StartCoroutine(ThrowContainerDelay());
        }
    }

    private IEnumerator AimRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_aimingTime);

        _isAiming = true;

        yield return wait;

        _isAiming = false;
        _canThrowContainer = true;
    }

    private IEnumerator ThrowContainerDelay()
    {
        WaitForSeconds wait = new WaitForSeconds(_containerThrowDelay);

        _isThrowContainerDelay = true;
        _currentContainer = null;

        yield return wait;

        _isContainerMoveToBoss = false;
        _isThrowContainerDelay = false;
    }

    private void PhaseTwo()
    {
        if (_canChangeGravity)
            StartCoroutine(ChangeGravityRoutine());
    }

    private IEnumerator ChangeGravityRoutine()
    {
        _canChangeGravity = false;

        float timeBeforeFlick = _delayPerChangeGravity - _flickTime;
        int index = Random.Range(0, 4);
        GravityDirection direction = _gravityDirection[index];

        _gravityArrows[index].gameObject.SetActive(true);

        if (timeBeforeFlick > 0)
        {
            yield return new WaitForSeconds(timeBeforeFlick);
        }

        _gravityArrows[index].StartFlicking();

        yield return new WaitForSeconds(_flickTime);

        _inverter.ChangeGravity(direction);
        _gravityArrows[index].gameObject.SetActive(false);

        _canChangeGravity = true;
    }

    private void PhaseThree()
    {
        Vector2 direction = (_player.transform.position - _laserStart.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        _laserStart.rotation = Quaternion.Slerp(_laserStart.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        Vector2 fireDirection = _laserStart.right;

        RaycastHit2D hit = Physics2D.Raycast(_laserStart.position, fireDirection, _maxDistance, _hitLayers);

        _aimLine.SetPosition(0, _laserStart.position);

        if (hit.collider != null)
        {
            _aimLine.SetPosition(1, hit.point);

            Player player = hit.collider.GetComponent<Player>();

            if (player != null)
            {
                _damageTimer += Time.deltaTime;

                if (_damageTimer >= _damageInterval)
                {
                    player.TakeDamage(_damage);
                    _damageTimer = 0f;
                }
            }
            else
            {
                _damageTimer = _damageInterval;
            }
        }
        else
        {
            _aimLine.SetPosition(1, (Vector2)_laserStart.position + (fireDirection * _maxDistance));
            _damageTimer = _damageInterval;
        }
    }
}