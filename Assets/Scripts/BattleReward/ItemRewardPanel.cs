using UnityEngine;
using System.Collections.Generic;

public class ItemRewardPanel : MonoBehaviour
{
    public Transform container;
    public GameObject itemPrefab;
    [SerializeField] private GameObject rewardsList;

    private ItemReward reward;
    private ItemPreviewUI previewUI;
    private PlayerInventory inventory;

    public void setItemPreview(ItemPreviewUI preview)
    {
        previewUI = preview;
    }

    public void setInventory(PlayerInventory inv)
    {
        inventory = inv;
    }

    public void setItems(List<ItemData> items, ItemReward reward)
    {
        this.reward = reward;

        foreach (Transform child in container)
            Destroy(child.gameObject);

        foreach (var item in items)
        {
            GameObject obj = Instantiate(itemPrefab, container);

            ItemClickUI ui = obj.GetComponent<ItemClickUI>();

            if (ui == null)
                continue;

            ui.setupReward(item, this, previewUI);
        }
    }

    public void selectItem(ItemData item)
    {
        if (item == null)
            return;

        inventory.addItem(item);

        if (reward != null)
            reward.complete();

        gameObject.SetActive(false);
        rewardsList.SetActive(true);
    }

    public void onCloseButtonClick()
    {
        rewardsList.SetActive(true);
        gameObject.SetActive(false);
    }
}