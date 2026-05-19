using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GravityArrow : MonoBehaviour
{
    private static readonly int CanFlickHash = Animator.StringToHash("canFlick");

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartFlicking()
    {
        _animator.SetBool(CanFlickHash, true);
    }

    private void OnDisable()
    {
        if (_animator != null)
        {
            _animator.SetBool(CanFlickHash, false);
        }
    }
}
