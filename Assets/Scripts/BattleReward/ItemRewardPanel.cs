using System.Collections.Generic;
using UnityEngine;

public class ItemRewardPanel : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject rewardsList;
    [SerializeField] private float itemScale = 2.5f;
    [SerializeField] private Sprite fullInventoryIcon;

    private ItemReward reward;
    private Tooltip tooltip;
    private PlayerInventory inventory;

    public void init(Tooltip tooltip, PlayerInventory inventory)
    {
        this.tooltip = tooltip;
        this.inventory = inventory;
    }

    public void setItems(List<ItemData> items, ItemReward reward)
    {
        this.reward = reward;

        foreach (Transform child in container)
            Destroy(child.gameObject);

        foreach (var item in items)
        {
            GameObject obj = Instantiate(itemPrefab, container);
            obj.transform.localScale = Vector3.one * itemScale;
            ItemSlotUI ui = obj.GetComponent<ItemSlotUI>();
            if (ui == null) continue;
            ui.setupReward(item, this, tooltip);
        }
    }

    public void selectItem(ItemData item)
    {
        if (item == null) return;

        if (!inventory.addItem(item))
        {
            Vector3 savedPos = tooltip.transform.position;
            //tooltip.hide();
            tooltip.show(new List<(Sprite, string, string)>
            {
                (fullInventoryIcon, "Ekwipunek pełny", $"Masz już {inventory.maxItems}/{inventory.maxItems} przedmiotów.\nWyrzuć coś (PPM) żeby zrobić miejsce.")
            });
            tooltip.transform.position = savedPos;
            return;
        }

        tooltip?.hide();
        reward?.complete();
        gameObject.SetActive(false);
        rewardsList.SetActive(true);
    }

    public void onCloseButtonClick()
    {
        tooltip?.hide();
        rewardsList.SetActive(true);
        gameObject.SetActive(false);
    }
}