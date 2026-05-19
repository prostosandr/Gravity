using System;
using UnityEngine;

[RequireComponent(typeof(BulletSpawner))]
[RequireComponent(typeof(GunReloader))]
[RequireComponent(typeof(GunMagazine))]
[RequireComponent(typeof(GunMuzzleFlash))]
[RequireComponent(typeof(GunRecoil))]
public class Gun : MonoBehaviour
{
    [SerializeField] private float _reloadTime;
    [SerializeField] private int _magazineCapacity;
    [SerializeField] private float _flashTime;
    [SerializeField] private float _recoilAngle;
    [SerializeField] private float _returnSpeed;

    private BulletSpawner _spawner;
    private GunReloader _reloader;
    private GunMagazine _magazine;
    private GunMuzzleFlash _flash;
    private GunRecoil _recoil;

    public event Action Shooted;
    public event Action<int, int> AmmoChanged;
    public event Action<float> ReloadStarted;

    public bool CanShoot => _magazine.IsMagazineEmpty == false && _reloader.IsReload == false;

    private void Awake()
    {
        _spawner = GetComponent<BulletSpawner>();
        _reloader = GetComponent<GunReloader>();
        _magazine = GetComponent<GunMagazine>();
        _flash = GetComponent<GunMuzzleFlash>();
        _recoil = GetComponent<GunRecoil>();
    }

    public void Initialize()
    {
        _reloader.Initialize(_reloadTime);
        _magazine.Initialize(_magazineCapacity);
        _flash.Initialize(_flashTime);
        _recoil.Initialize(_recoilAngle, _returnSpeed);

        AmmoChanged?.Invoke(_magazine.NumberOfBullets, _magazineCapacity);
    }

    private void OnEnable()
    {
        _reloader.ReloadStarted += OnReloadStarted;
        _reloader.ReloadCompleted += OnReloadFinished;
    }

    private void OnDisable()
    {
        _reloader.ReloadStarted -= OnReloadStarted;
        _reloader.ReloadCompleted -= OnReloadFinished;
    }

    public void Shoot()
    {
        if (_magazine.IsMagazineEmpty == false && _reloader.IsReload == false)
        {
            _spawner.Spawn();
            _magazine.UseBullet();
            _flash.Flash();
            _recoil.ApplyRecoil();

            Shooted?.Invoke();
            AmmoChanged?.Invoke(_magazine.NumberOfBullets, _magazineCapacity);
        }
        else if (_magazine.IsMagazineEmpty && _reloader.IsReload == false)
        {
            Reload();
        }
    }

    public void Reload()
    {
        if (_reloader.IsReload == false && _magazine.IsMagazineFull == false)
        {
            _reloader.Reload();

            AmmoChanged?.Invoke(_magazine.NumberOfBullets, _magazineCapacity);
        }
    }

    public void OnReloadStarted(float reloadTime)
    {
        ReloadStarted?.Invoke(reloadTime);
    }

    public void OnReloadFinished()
    {
        _magazine.LoadBullets();
        AmmoChanged?.Invoke(_magazine.NumberOfBullets, _magazineCapacity);
    }

    public void UpdateRecoil()
    {
        _recoil.UpdateRecoil();
    }
}
