using UnityEngine;

public class GravityInverterUI : MonoBehaviour
{
    [SerializeField] private GravityInverter _inverter;
    [SerializeField] private ReloadBar _bar;

    private void Awake()
    {
        _bar.Initialize();
    }

    private void OnEnable()
    {
        _inverter.CooldownStarted += _bar.StartLoading;
    }

    private void OnDisable()
    {
        _inverter.CooldownStarted -= _bar.StartLoading;
    }
}
