using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossContainer : MonoBehaviour
{
    [SerializeField] private BossContainerTrigger _trigger;
    [SerializeField] private float _damage;

    private Rigidbody2D _rigidbody;

    private bool _canAttack;

    public Rigidbody2D Rigidbody => _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _canAttack = false;
    }

    private void OnEnable()
    {
        _trigger.OnTriggerEntered += Attack;
    }

    private void OnDisable()
    {
        _trigger.OnTriggerEntered -= Attack;
    }

    private void Attack(Player player)
    {
        if (_canAttack)
        {
            player.TakeDamage(_damage);

            _canAttack = false;
        }
    }

    public void SetKinematic()
    {
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

    public void SetDynamic()
    {
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    public void ActivateAttack()
    {
        _canAttack = true;
    }

    public void DeactivateAttack()
    {
        _canAttack = false;
    }
}
