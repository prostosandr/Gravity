using UnityEngine;

public class GravityWell : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer _vizualZone;
    [SerializeField] private LayerMask _hitLayers;

    [SerializeField] private float _maxDistance;
    [SerializeField] private float _raycastOffest;

    public void Initialize(float width)
    {
        Vector2 rayStart = transform.position + transform.up * _raycastOffest;

        RaycastHit2D hit = Physics2D.Raycast(rayStart, transform.up, _maxDistance, _hitLayers);

        float distance;

        if(hit.collider != null)
            distance = hit.distance;
        else
            distance = _maxDistance;

        _collider.size = new Vector2(width, distance);
        _collider.offset = new Vector2(0, distance / 2f);

        _vizualZone.size = new Vector2(width, distance);
    }
}
