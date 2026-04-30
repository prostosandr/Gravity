using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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