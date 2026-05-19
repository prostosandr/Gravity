using UnityEditor;
using UnityEngine;

public class ColliderChecker : MonoBehaviour
{
    [SerializeField] private Vector2 _size;
    [SerializeField] private LayerMask _layers;

    public bool CheckCollider()
    {
        return Physics2D.OverlapBox(transform.position, _size, transform.eulerAngles.z, _layers);
    }

    public Collider2D GetObject()
    {
        return Physics2D.OverlapBox(transform.position, _size, transform.eulerAngles.z, _layers);
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

        using (new Handles.DrawingScope(rotationMatrix))
        {
            Handles.DrawWireCube(Vector3.zero, _size);
        }
    }
}