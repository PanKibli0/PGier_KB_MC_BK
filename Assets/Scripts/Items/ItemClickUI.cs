using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemClickUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private ItemData item;
    private ItemPreviewUI previewUI;
    private ItemRewardPanel panel;

    private int index;

    private ItemRewardPanel rewardPanel;
    private bool isRewardMode = false;
    public void setup(ItemData item, int index)
    {
        this.item = item;
        this.index = index;
        isRewardMode = false;
    }

    public void setupReward(ItemData item, ItemRewardPanel panel)
    {
        this.item = item;
        this.panel = panel;
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

        if (panel != null)
        {
            Debug.Log("HOVER REWARD ITEM: " + item.itemName);
        }
        else
        {
            Debug.Log("HOVER INVENTORY ITEM: " + item.itemName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (previewUI != null)
            previewUI.clear();
    }
}