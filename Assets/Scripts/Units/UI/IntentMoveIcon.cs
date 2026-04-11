using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntentMoveIcon : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text valueText;

    public void init(Sprite iconSprite, string value)
    {
        if (icon != null)
            icon.sprite = iconSprite;

        if (valueText != null)
            valueText.text = value;
    }
}