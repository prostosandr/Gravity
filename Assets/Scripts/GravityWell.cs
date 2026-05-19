using System.Collections;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
    private static readonly int CanFlickHash = Animator.StringToHash("canFlick");

    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer _vizualZone;
    [SerializeField] private LayerMask _hitLayers;

    [SerializeField] private float _maxDistance;
    [SerializeField] private float _raycastOffest;

    [SerializeField] private Animator _animator;
    [SerializeField] private float _abilityTime;
    [SerializeField] private float _flickTime;

    private WaitForSeconds _wait;
    private Coroutine _coroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _wait = new WaitForSeconds(_abilityTime);
    }

    private void OnEnable()
    {
        _animator.SetBool(CanFlickHash, false);
    }


    public void Initialize(float width)
    {
        Vector2 rayStart = transform.position + transform.up * _raycastOffest;

        RaycastHit2D hit = Physics2D.Raycast(rayStart, transform.up, _maxDistance, _hitLayers);

        float distance;

        if (hit.collider != null)
            distance = hit.distance;
        else
            distance = _maxDistance;

        _collider.size = new Vector2(width, distance);
        _collider.offset = new Vector2(0, distance / 2f);

        _vizualZone.size = new Vector2(width, distance);
    }

    public void StartAnimation()
    {
        _animator.SetBool(CanFlickHash, false);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        float timeBeforeFlick = _abilityTime - _flickTime;

        if (timeBeforeFlick > 0)
            yield return new WaitForSeconds(timeBeforeFlick);

        _animator.SetBool(CanFlickHash, true);

        yield return new WaitForSeconds(_flickTime);

        _animator.SetBool(CanFlickHash, false);
        _vizualZone.enabled = true;
        gameObject.SetActive(false);    
    }
}
