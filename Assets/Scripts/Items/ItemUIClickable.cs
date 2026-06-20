using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemUIClickable : MonoBehaviour, IPointerClickHandler
{
    public event Action<ItemData> OnClick;

    [Header("UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;

    private ItemData itemData;

    public void init(ItemData data)
    {
        itemData = data;

        if (iconImage != null)
            iconImage.sprite = data.icon;

        if (nameText != null)
            nameText.text = data.itemName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(itemData);
    }
}