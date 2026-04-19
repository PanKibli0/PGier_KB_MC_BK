using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntentMoveIcon : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text valueText;

    public void init(string iconPath, string value)
    {
        if (!string.IsNullOrEmpty(iconPath))
        {
            Sprite sprite = Resources.Load<Sprite>(iconPath);
            if (sprite != null)
                icon.sprite = sprite;
        }

        if (valueText != null)
            valueText.text = value;
    }
}