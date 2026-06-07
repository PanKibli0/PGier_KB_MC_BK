using UnityEngine;
using System.Collections.Generic;

public class ItemReward : BaseReward
{
    private List<ItemData> items;
    private ItemRewardPanel panel;
    private GameObject rewardsList;

    public void init(List<ItemData> items, ItemRewardPanel panel, GameObject rewardsList)
    {
        this.items = items;
        this.panel = panel;
        this.rewardsList = rewardsList;
    }

    public override void collect()
    {
        rewardsList.SetActive(false);
        panel.setItems(items, this);
        panel.gameObject.SetActive(true);
    }

    public void complete()
    {
        button.destroyButton();
        rewardsList.SetActive(true);
    }

    public override string getDescription()
    {
        return "Zdobadz przedmiot";
    }

    public override Sprite getIcon()
    {
        return Resources.Load<Sprite>("Icons_map/sklep");
    }
}