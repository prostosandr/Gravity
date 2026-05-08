using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Gun _gun;
    [SerializeField] private ReloadBar _reloadBar;
    [SerializeField] private AmmoCounter _ammo;

    private void Awake()
    {
        _reloadBar.Initialize();
    }

    private void OnEnable()
    {
        _gun.AmmoChanged += _ammo.UpdateText;
        _gun.ReloadStarted += _reloadBar.StartLoading;
    }

    private void OnDisable()
    {
        _gun.AmmoChanged -= _ammo.UpdateText;
        _gun.ReloadStarted -= _reloadBar.StartLoading;
    }
}
