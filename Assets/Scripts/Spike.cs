using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _knockbackForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IDamageable item))
        {
            item.TakeDamage(_damage);

            if(collision.TryGetComponent(out Rigidbody2D rigidbody))
            {
                Vector2 direction = (collision.transform.position - transform.position).normalized;
                direction = (direction + (Vector2)transform.up).normalized;

                rigidbody.linearVelocity = Vector2.zero;
                rigidbody.AddForce(direction * _knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}