using UnityEngine;

public class GravityWellInteraction : MonoBehaviour, IAbility
{
    [SerializeField] private GravityWell _gravityWell;
    [SerializeField] private GravityWellPreview _gravityWellPreview;
    [SerializeField] private LineRenderer _aimLine;
    [SerializeField] private LayerMask _hitLayers;
    [SerializeField] private float _maxDistance;

    [SerializeField] private float _minWidth;
    [SerializeField] private float _maxWidth;
    [SerializeField] private float _changeSpeed;
    [SerializeField] private float _currentWidth;

    private Vector2 _currentHitPosition;
    private Quaternion _currentRotation;

    public void Cast(bool isAbilityHeld)
    {
        PlaceGravityWell(isAbilityHeld);
    }

    public void StartAbility()
    {
        _aimLine.gameObject.SetActive(true);
    }

    public void AimAbility(Vector2 aim, bool isGamepad, float radius)
    {
        UpdatePreview(aim, isGamepad, radius);
    }

    public void CancelAbility()
    {
        DeactivatePreview();
        DeactivateAimLine();
    }

    public void UpdatePreview(Vector2 input, bool isGamePad, float radiusInput)
    {
        _currentWidth += radiusInput * _changeSpeed * Time.deltaTime;
        _currentWidth = Mathf.Clamp(_currentWidth, _minWidth, _maxWidth);

        Vector2 orign = transform.position;
        Vector2 direction;

        if (isGamePad)
            direction = input.normalized;
        else
            direction = (input - orign).normalized;

        RaycastHit2D hit = Physics2D.Raycast(orign, direction, _maxDistance, _hitLayers);
        
        _aimLine.SetPosition(0, orign);

        if (hit.collider != null)
        {
            _gravityWellPreview.gameObject.SetActive(true);
            _gravityWellPreview.transform.position = hit.point;

            _gravityWellPreview.PrewievRenderer.size = new Vector2(_currentWidth, _gravityWellPreview.PrewievRenderer.size.y);

            _aimLine.SetPosition(1, hit.point);

            float angle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg;
            _currentRotation = Quaternion.Euler(0, 0, angle - 90);
            _gravityWellPreview.transform.rotation = _currentRotation;

            _currentHitPosition = hit.point;
        }
        else
        {
            Vector2 endPoint = orign + (direction * _maxDistance);
            _aimLine.SetPosition(1, endPoint);

            DeactivatePreview();
        }
    }

    private void DeactivatePreview()
    {
        if (_gravityWellPreview.gameObject.activeSelf)
            _gravityWellPreview.gameObject.SetActive(false);
    }

    private void DeactivateAimLine()
    {
        if (_aimLine.gameObject.activeSelf)
            _aimLine.gameObject.SetActive(false);
    }

    private void PlaceGravityWell(bool canPlaceGravityWell)
    {
        if (canPlaceGravityWell && _gravityWellPreview.gameObject.activeSelf)
        {
            _gravityWell.gameObject.SetActive(true);
            _gravityWell.transform.position = _currentHitPosition;
            _gravityWell.transform.rotation = _currentRotation;
            _gravityWell.Initialize(_currentWidth);
        }
    }
}
