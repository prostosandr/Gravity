using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    [SerializeField] private Gravity _gravity;
    [SerializeField] private List<Transform> _wayPoints;
    [SerializeField] private float _minDistance = 0.5f;
    [SerializeField] private float _speed;
    [SerializeField] private float _interpolationSpeed;

    [Header("Detection")]
    [SerializeField] private float _visionRadius;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _obstacleLayer;

    [Header("Combat")]
    [SerializeField] private WeaponHandler _weaponHandler;
    [SerializeField] private float _shootDelay; 
    [SerializeField] private float _burstDelay; 
    [SerializeField] private int _burstSize;

    [SerializeField] private float _maxHealth;

    private Rigidbody2D _rigidbody;
    private Mover _mover;
    private Rotator _rotator;
    private Health _health;

    private Transform _currentWayPoint;
    private int _currentIndex;
    private Transform _targetPlayer;

    private bool _isShooting;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _mover = GetComponent<Mover>();
        _rotator = GetComponent<Rotator>();
        _health = GetComponent<Health>();

        _currentIndex = 0;
        _currentWayPoint = GetNextWayPoint();

        _mover.Initialize(_rigidbody, _speed, _interpolationSpeed);

        if (_weaponHandler != null)
            _weaponHandler.Initialize();
        else
            Debug.LogError($"WeaponHandler не назначен на объекте {gameObject.name}!");

        _health.Initialize(_maxHealth);
    }

    private void Update()
    {
        CheckPlayer();

        if (_targetPlayer != null)
        {
            Vector2 aimDirection = (_targetPlayer.position - transform.position).normalized;

            _weaponHandler.Rotate(aimDirection);
            _weaponHandler.GunUpdate();

            LookAtPlayer();
        }
        else
        {
            Patrol();
        }
    }

    public void TakeDamage(float damage)
    {
        _health.TakeDamage(damage);
    }

    private void CheckPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _visionRadius, _playerLayer);

        if (hit != null)
        {
            Vector2 directionToPlayer = (hit.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, hit.transform.position);

            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, _playerLayer | _obstacleLayer);

            if (rayHit.collider != null && ((1 << rayHit.collider.gameObject.layer) & _playerLayer) != 0)
            {
                _targetPlayer = hit.transform;
            }
            else
            {
                _targetPlayer = null;
            }
        }
        else
        {
            _targetPlayer = null;
        }
    }

    private void LookAtPlayer()
    {
        _mover.Move(Vector2.zero, _gravity.CurrentDirection);

        Vector2 directionToPlayer = (_targetPlayer.position - transform.position).normalized;
        _rotator.TurnForward(directionToPlayer, _gravity.CurrentDirection);

        if (!_isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        _isShooting = true;

        for (int i = 0; i < _burstSize; i++)
        {
            _weaponHandler.Shoot();
            yield return new WaitForSeconds(_burstDelay);
        }

        yield return new WaitForSeconds(_shootDelay);

        _isShooting = false;
    }

    private void Patrol()
    {
        if (_currentWayPoint == null) return;

        if ((transform.position - _currentWayPoint.position).sqrMagnitude > _minDistance * _minDistance)
        {
            Vector2 moveDirection = (_currentWayPoint.position - transform.position).normalized;
            _mover.Move(moveDirection, GravityDirection.Down);
            _rotator.TurnForward(moveDirection, GravityDirection.Down);
        }
        else
        {
            _currentWayPoint = GetNextWayPoint();
        }
    }

    private Transform GetNextWayPoint()
    {
        if (_wayPoints == null || _wayPoints.Count == 0) return null;

        Transform nextWayPoint = _wayPoints[_currentIndex];
        _currentIndex = (_currentIndex + 1) % _wayPoints.Count;
        return nextWayPoint;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _visionRadius);
    }
}