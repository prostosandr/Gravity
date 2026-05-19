using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour
{
    [SerializeField] private DoorTrigger _doorTrigger;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (_doorTrigger != null)
            _doorTrigger.OnTriggerEntered += Close;
    }

    private void OnDisable()
    {
        if (_doorTrigger != null)
            _doorTrigger.OnTriggerEntered -= Close;
    }

    public void Close()
    {
        _animator.SetBool("Open", false);
    }

    public void Open()
    {
        _animator.SetBool("Open", true);
    }
}