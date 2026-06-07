using UnityEngine;
using System.Collections.Generic;

public class RelicReward : BaseReward
{
    public List<RelicData> relics;
    public RelicRewardPanel panel;
    public GameObject rewardsList;

    public override void collect()
    {
        rewardsList.SetActive(false);
        panel.setRelics(relics, this);
        panel.gameObject.SetActive(true);
    }

    public void complete()
    {
        button.destroyButton();
        rewardsList.SetActive(true);
    }

    public override string getDescription()
    {
        return "Zdobadz relikt";
    }

    public override Sprite getIcon()
    {
        return Resources.Load<Sprite>("Icons/skarb");
    }
}