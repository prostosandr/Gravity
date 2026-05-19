using UnityEngine;

public class DamageButton : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _damage;

    public void OnButtonClick()
    {
        _player.TakeDamage(_damage);
    }
}
