using UnityEngine;

public class PickUpLogic : MonoBehaviour
{
    [SerializeField] private ColliderChecker _objectCheker;
    [SerializeField] private Transform _textPanel;
    [SerializeField] private Transform _hand;

    private Collider2D _currentObject;
    bool _isObject;

    private void Update()
    {
        if (_objectCheker.CheckCollider())
        {
            _textPanel.gameObject.SetActive(true);
            _isObject = true;
        }
        else
        {
            _textPanel.gameObject.SetActive(false);
            _isObject = false;
        }
    }

    public void PickUpObject()
    {
        if (_currentObject == null)
        {
            if (_isObject)
            {
                _currentObject = _objectCheker.GetObject();

                _currentObject.transform.SetParent(transform);
                _currentObject.transform.position = _hand.transform.position;

                if (_currentObject.TryGetComponent(out Rigidbody2D rigidbody))
                    rigidbody.simulated = false;
            }
        }
        else
        {
            if (_currentObject.TryGetComponent(out Rigidbody2D rigidbody))
                rigidbody.simulated = true;

            _currentObject.transform.SetParent(null);
            _currentObject = null;
        }
    }
}
