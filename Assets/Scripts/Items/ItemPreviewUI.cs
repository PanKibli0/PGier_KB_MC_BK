using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPreviewUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void show(ItemData item)
    {
        rectTransform.position = Input.mousePosition;
        icon.sprite = item.icon;
        nameText.text = item.itemName;
        descriptionText.text = item.getDescription();
    }

    public void clear()
    {
        icon.sprite = null;
        nameText.text = "";
        descriptionText.text = "";
    }
}