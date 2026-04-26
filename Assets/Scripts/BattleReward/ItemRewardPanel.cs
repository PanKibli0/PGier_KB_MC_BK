using UnityEngine;
using System.Collections.Generic;

public class ItemRewardPanel : MonoBehaviour
{
    public Transform container;
    public GameObject itemPrefab;
    [SerializeField] private GameObject rewardsList;

    private ItemReward reward;

    public void setItems(List<ItemData> items, ItemReward reward)
    {
        this.reward = reward;

        foreach (Transform child in container)
            Destroy(child.gameObject);

        foreach (var item in items)
        {
            Debug.Log("PANEL ITEM: " + item.itemName);
            GameObject obj = Instantiate(itemPrefab, container);

            ItemClickUI ui = obj.GetComponent<ItemClickUI>();

            if (ui == null)
            {
                Debug.LogError("ItemPrefab nie ma ItemClickUI!");
                continue;
            }

            ui.setupReward(item, this);
        }
    }

    public void selectItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogError("selectItem: item == null");
            return;
        }

        PlayerInventory.Instance.AddItem(item);

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