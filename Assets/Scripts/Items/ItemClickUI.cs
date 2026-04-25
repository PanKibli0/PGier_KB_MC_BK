using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ItemClickUI : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    private ItemData item;
    private int index;

    public void setup(ItemData item, int index)
    {
        this.item = item;
        this.index = index;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Tooltip instance: " + Tooltip.Instance);
        if (Tooltip.Instance == null || item == null) return;

        var entries = new List<(Sprite, string, string)>
        {
            (item.icon, item.itemName, item.description)
        };

        Tooltip.Instance.show(entries);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Tooltip.Instance == null) return;

        Tooltip.Instance.hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;

        Debug.Log("U¿yto itemu: " + item.itemName);

        PlayerInventory.Instance.UseItem(index);

        if (Tooltip.Instance != null)
            Tooltip.Instance.hide();
    }
}