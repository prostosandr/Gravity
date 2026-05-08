using TMPro;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void UpdateText(int currentBullets, int magazineCapasity)
    {
        _text.text = $"{currentBullets}/{magazineCapasity}";
    }
}
