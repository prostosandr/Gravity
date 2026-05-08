using System;
using UnityEngine;

[RequireComponent(typeof(WeaponHandlerRotator))]
public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Gun _gun;
    [SerializeField] private PlayerInputProvider _input;

    private WeaponHandlerRotator _aimer;

    private bool _isGravityInverted;

    public void Initialize()
    {
        _aimer = GetComponent<WeaponHandlerRotator>();

        _gun.Initialize();
    }

    public void Rotate()
    {
        _aimer.Rotate(_isGravityInverted, _input.GetAimDirection(transform.position));
    }

    public void Shoot()
    {
        _gun.Shoot();
    }

    public void Reload()
    {
        _gun.Reload();
    }

    public void OnGravityInvert(bool isGravityInverted)
    {
        _isGravityInverted = isGravityInverted;
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
