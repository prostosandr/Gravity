using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OvverheadKickChecker : MonoBehaviour
{
    [SerializeField] private float _crushMassTreshold;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private float _rayDistance;
    [SerializeField] private float _hitDistance;
    [SerializeField] private Health _health;

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, _rayDistance, _layer);

        Debug.DrawRay(transform.position, transform.up, Color.red);

        if (hit.collider != null && hit.rigidbody != null)
        {
            if (hit.rigidbody.mass >= _crushMassTreshold && hit.distance < _hitDistance)
            {
                _health.Die();
            }
        }
    }

    
}
