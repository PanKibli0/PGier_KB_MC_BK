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
    private PlayerInventory inventory;
    private Unit playerUnit;

    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    public void setup(ItemData item, int index, ItemPreviewUI previewUI, PlayerInventory inventory, Unit playerUnit)
    {
        this.item = item;
        this.index = index;
        this.previewUI = previewUI;
        this.inventory = inventory;
        this.playerUnit = playerUnit;
        refreshUI();
    }

    public void setupReward(ItemData item, ItemRewardPanel panel, ItemPreviewUI previewUI)
    {
        this.item = item;
        this.panel = panel;
        this.previewUI = previewUI;
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
            inventory.useItem(index, playerUnit);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;
        if (previewUI != null)
            previewUI.show(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (previewUI != null)
            previewUI.clear();
    }
}