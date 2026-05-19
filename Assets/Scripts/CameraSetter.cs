using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSetter : MonoBehaviour
{
    [SerializeField] private float _fieldOfViewSpeed;

    private Camera _camera;

    private float _newFiledOfView;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        _newFiledOfView = _camera.fieldOfView;
    }

    private void Update()
    {
        if (_camera.fieldOfView != _newFiledOfView)
            _camera.fieldOfView = Mathf.MoveTowards(_camera.fieldOfView, _newFiledOfView, _fieldOfViewSpeed * Time.deltaTime);
    }

    public void SetFieldOfView(float value)
    {
        _newFiledOfView = value;
    }
}
