using UnityEngine;

[RequireComponent(typeof(WeaponHandlerRotator))]
public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Gun _gun;

    private WeaponHandlerRotator _aimer;

    private GravityDirection _currentGravity = GravityDirection.Down;

    public void Initialize()
    {
        _aimer = GetComponent<WeaponHandlerRotator>();
        _gun.Initialize();
    }

    public void Rotate(Vector2 aiDirection)
    {
        _aimer.Rotate(_currentGravity, aiDirection);
    }

    public void Shoot()
    {
        _gun.Shoot();
    }

    public void Reload()
    {
        _gun.Reload();
    }

    public void OnGravityChanged(GravityDirection gravityDirection)
    {
        _currentGravity = gravityDirection;
    }

    public void GunUpdate()
    {
        _gun.UpdateRecoil();
    }

    public void Activate()
    {
        gameObject.SetActive(false);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}