using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipEntry : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;



    public void init(Sprite iconSprite, string name, string description)
    {
        if (icon != null)
            icon.sprite = iconSprite;

        if (nameText != null)
            nameText.text = name;

        if (descriptionText != null)
            descriptionText.text = description;
    }


    public float getHeight()
    {
        float descriptionHeight = descriptionText != null ? descriptionText.preferredHeight : 0f;
        float total = 50f + descriptionHeight;
        return total;
    }
}