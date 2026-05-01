using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPreviewUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    public void show(ItemData item)
    {
        icon.sprite = item.icon;
        nameText.text = item.itemName;
        descriptionText.text = item.description;
    }

    public void clear()
    {
        icon.sprite = null;
        nameText.text = "";
        descriptionText.text = "";
    }
}