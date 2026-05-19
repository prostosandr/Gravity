using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ZeroGPropulsion : MonoBehaviour
{
    [SerializeField] private PlayerInputProvider _input;
    [SerializeField] private Gravity _gravity;
    [SerializeField] private float _recoilThrust;
    [SerializeField] private Gun _gun;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _input.ShootPressed += ApplyJetRecoil;
    }

    private void OnDisable()
    {
        _input.ShootPressed -= ApplyJetRecoil;
    }

    private void ApplyJetRecoil(bool isCastAbilityHeld)
    {
        if (_gun.CanShoot == false)
            return;

        if (_gravity.CurrentDirection == GravityDirection.Zero && !isCastAbilityHeld)
        {
            Vector2 aimDirection = _input.GetAimDirection(transform.position);

            _rigidbody.linearVelocity = Vector2.zero; 
            _rigidbody.AddForce(-aimDirection * _recoilThrust, ForceMode2D.Impulse);
        }
    }
}