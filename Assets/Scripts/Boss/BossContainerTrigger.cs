using System;
using UnityEngine;

public class BossContainerTrigger : MonoBehaviour
{
    public event Action<Player> OnTriggerEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Player player))
            OnTriggerEntered?.Invoke(player);
    }
}
