using UnityEngine;

public class GravityArrowsMover : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    public void Update()
    {
        if (_target != null)
            transform.position = _target.position + _offset;
    }
}
