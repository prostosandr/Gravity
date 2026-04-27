using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _blood;
    [SerializeField] private float _crushMassTreshold;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private float _rayDistance;
    [SerializeField] private float _hitDistance;


    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, _rayDistance, _layer);

        Debug.DrawRay(transform.position, transform.up, Color.red);

        if (hit.collider != null && hit.rigidbody != null)
        {
            if (hit.rigidbody.mass >= _crushMassTreshold && hit.distance < _hitDistance)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        _blood.transform.SetParent(null);
        _blood.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }
}
