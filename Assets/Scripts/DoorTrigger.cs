using System;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public event Action OnTriggerEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            OnTriggerEntered?.Invoke();
        }
    }
}