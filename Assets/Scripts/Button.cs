using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Door _door;

    private int _numberOfObjects;

    private void Awake()
    {
        _numberOfObjects = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _numberOfObjects++;

        if (_numberOfObjects > 0)
            _door.Open();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _numberOfObjects--;

        if (_numberOfObjects == 0)
            _door.Close();
    }
}
