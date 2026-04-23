using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _followSpeed;

    [Header("Offset")]
    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _zDistance;

    private void Start()
    {
        transform.position = GetTargetPosition();
    }

    private void LateUpdate()
    {
        if (_target == null)
            return;

        Vector3 targetPos = GetTargetPosition();

        transform.position = Vector3.Lerp(transform.position, targetPos, _followSpeed * Time.deltaTime);
    }

    private Vector3 GetTargetPosition()
    {
        return new Vector3(
            _target.position.x + _offset.x,
            _target.position.y + _offset.y,
            _zDistance
        );
    }
}