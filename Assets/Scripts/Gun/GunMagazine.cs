using UnityEngine;

public class GunMagazine : MonoBehaviour
{
    private int _numberOfBullets;
    private int _magazineCapacity;

    public int NumberOfBullets => _numberOfBullets;
    public int MagazineCapacity => _magazineCapacity;
    public bool IsMagazineEmpty => _numberOfBullets == 0;
    public bool IsMagazineFull => _numberOfBullets == _magazineCapacity;

    public void Initialize(int magazineCapacity)
    {
        _magazineCapacity = magazineCapacity;

        LoadBullets();
    }
    
    public void UseBullet()
    {
        _numberOfBullets--;
    }

    public void LoadBullets()
    {
        _numberOfBullets = _magazineCapacity;
    }
}