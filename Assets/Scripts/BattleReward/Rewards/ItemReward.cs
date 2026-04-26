using UnityEngine;
using System.Collections.Generic;

public class ItemReward : BaseReward
{
    public List<ItemData> items;
    public ItemRewardPanel panel;
    public GameObject rewardsList;

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
        return "Zdobπdü przedmiot";
    }

    public override Sprite getIcon()
    {
        return null;
    }
}