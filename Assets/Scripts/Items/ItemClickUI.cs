using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemClickUI : MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    private ItemData item;
    private ItemRewardPanel panel;
    private int index;
    private ItemPreviewUI previewUI;

    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    public void setup(ItemData item, int index)
    {
        this.item = item;
        this.index = index;
        previewUI = FindObjectOfType<ItemPreviewUI>();
        refreshUI();
    }

    public void setupReward(ItemData item, ItemRewardPanel panel)
    {
        this.item = item;
        this.panel = panel;
        previewUI = FindObjectOfType<ItemPreviewUI>();
        refreshUI();
    }

    private void refreshUI()
    {
        if (item == null) return;

        if (icon != null)
            icon.sprite = item.icon;

        if (nameText != null)
            nameText.text = item.itemName;

        if (descriptionText != null)
            descriptionText.text = item.description;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (panel != null)
        {
            panel.selectItem(item);
        }
        else
        {
            PlayerInventory.Instance.UseItem(index);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;

        if (ItemPreviewUI.Instance != null)
            ItemPreviewUI.Instance.show(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (previewUI == null)
            previewUI = Object.FindFirstObjectByType<ItemPreviewUI>();

        if (previewUI != null)
            previewUI.clear();
    }
}