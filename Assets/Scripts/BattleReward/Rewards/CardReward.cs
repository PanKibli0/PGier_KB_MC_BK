using UnityEngine;
using System.Collections.Generic;

public class CardReward : BaseReward
{
    public List<CardData> cards;
    public CardRewardPanel panel;
    public GameObject rewardsList;

    public override void collect()
    {
        rewardsList.SetActive(false);
        panel.setCards(cards, this);
        panel.gameObject.SetActive(true);
    }

    public void complete()
    {
        button.destroyButton();
        rewardsList.SetActive(true);
    }

    public override string getDescription()
    {
        return "Zdobądź kartę";
    }

    public override Sprite getIcon()
    {
        return null;
    }
}