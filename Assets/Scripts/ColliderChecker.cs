using UnityEditor;
using UnityEngine;

public class ColliderChecker : MonoBehaviour
{
    [SerializeField] private Vector2 _size;
    [SerializeField] private LayerMask _layer;

    public bool CheckCollider()
    {
        return Physics2D.OverlapBox(transform.position, _size, 0f, _layer);
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireCube(transform.position, _size);
    }
}