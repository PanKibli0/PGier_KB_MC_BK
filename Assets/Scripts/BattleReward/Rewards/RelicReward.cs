using UnityEngine;
using System.Collections.Generic;

public class RelicReward : BaseReward
{
    private List<RelicData> relics;
    private RelicRewardPanel panel;
    private GameObject rewardsList;

    public void init(List<RelicData> relics, RelicRewardPanel panel, GameObject rewardsList)
    {
        this.relics = relics;
        this.panel = panel;
        this.rewardsList = rewardsList;
    }

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
        return Resources.Load<Sprite>("Icons_map/skarb");
    }
}