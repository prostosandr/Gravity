using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explodeRadius;
    [SerializeField] private float _explodeForce;
    [SerializeField] private float _explodeTime;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private ParticleSystem _explodeEffect;

    private Collider2D[] _hitColliders;
    private ContactFilter2D _filter;
    private WaitForSeconds _wait;
    private Coroutine _coroutine;

    private void Awake()
    {
        _hitColliders = new Collider2D[50];

        _filter.useLayerMask = true;
        _filter.layerMask = _layerMask;
        _filter.useTriggers = false;

        _wait = new WaitForSeconds(_explodeTime);
    }

    public void StartExplode()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ExplodeRoutine());
    }

    public void Explode()
    {
        int numberOfColliders = Physics2D.OverlapCircle(transform.position, _explodeRadius, _filter, _hitColliders);

        for (int i = 0; i < numberOfColliders; i++)
        {
            Rigidbody2D rigidbody = _hitColliders[i].GetComponent<Rigidbody2D>();

            if (rigidbody != null)
            {
                ApplyExplosionForce(rigidbody);
            }
        }

        _explodeEffect.transform.SetParent(null);
        _explodeEffect.transform.position = transform.position;
        _explodeEffect.Play();
        gameObject.SetActive(false);
    }

    private void ApplyExplosionForce(Rigidbody2D rigidbody)
    {
        Vector2 explosionPos = transform.position;
        Vector2 objectPos = rigidbody.position;
        Vector2 direction = objectPos - explosionPos;

        float distance = direction.magnitude;

        if (distance > 0)
        {
            float falloff = 1 - (distance / _explodeRadius);

            Vector2 force = direction.normalized * (_explodeForce * falloff);

            rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }

    private IEnumerator ExplodeRoutine()
    {
        yield return _wait;

        Explode();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explodeRadius);
    }
}
