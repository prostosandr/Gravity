using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _bar;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    public void Update()
    {
        if (_target != null)
            transform.position = _target.position + _offset;
    }

    public void UpdateBar(float health, float maxHealth)
    {
        _bar.fillAmount = health / maxHealth;
    }
}
