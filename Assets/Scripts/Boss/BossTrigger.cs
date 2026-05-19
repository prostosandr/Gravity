using System;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public event Action OnTriggerEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            OnTriggerEntered?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
