using UnityEngine;

public class Picker : MonoBehaviour
{
    [SerializeField] private ColliderChecker _objectCheker;
    [SerializeField] private Transform _handPivot;

    private Collider2D _currentObject;
    private Rigidbody2D _objectRigidbody;

    private float _throwPower;

    public void Initialize(float throwPower)
    {
        _throwPower = throwPower;
    }

    public void InteractObject()
    {
        if (_currentObject == null)
        {
            _currentObject = _objectCheker.GetObject();

            if (_currentObject != null && _currentObject.TryGetComponent(out _objectRigidbody))
                PickUpObject();
        }
        else
        {
            DetachObject();
            ClearHands();
        }
    }

    public void Throw(Vector2 direction)
    {
        if (_currentObject == null)
            return;

        if (_currentObject.TryGetComponent(out Bomb bomb))
            bomb.StartExplode();

        DetachObject();

        _objectRigidbody.AddForce(direction * _throwPower, ForceMode2D.Impulse);

        ClearHands();
    }

    private void PickUpObject()
    {
        _objectRigidbody.simulated = false;

        _currentObject.transform.SetParent(transform);
        _currentObject.transform.position = _handPivot.transform.position;
    }

    private void DetachObject()
    {
        _objectRigidbody.simulated = true;
        _currentObject.transform.SetParent(null);
        _objectRigidbody.linearVelocity = Vector2.zero;
    }

    private void ClearHands()
    {
        _currentObject = null;
        _objectRigidbody = null;
    }
}