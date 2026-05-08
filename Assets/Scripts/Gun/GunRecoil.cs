using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [SerializeField] private Transform _recoilPivot;

    private float _recoilAngle;
    private float _returnSpeed;
    private Quaternion _initialRotation;

    public void Initialize(float recoilAngle, float returnSpeed)
    {
        _recoilAngle = recoilAngle;
        _returnSpeed = returnSpeed;

        _initialRotation = transform.localRotation;
    }

    public void ApplyRecoil()
    {
        _recoilPivot.localRotation *= Quaternion.Euler(0, 0, _recoilAngle);
    }

    public void UpdateRecoil()
    {
        _recoilPivot.localRotation = Quaternion.Lerp(_recoilPivot.localRotation, _initialRotation, _returnSpeed * Time.deltaTime);
    }
}
