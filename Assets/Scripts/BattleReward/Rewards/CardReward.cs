using UnityEngine;
using System.Collections.Generic;

public class CardReward : BaseReward
{
    private List<CardData> cards;
    private CardRewardPanel panel;
    private GameObject rewardsList;

    public void init(List<CardData> cards, CardRewardPanel panel, GameObject rewardsList)
    {
        this.cards = cards;
        this.panel = panel;
        this.rewardsList = rewardsList;
    }

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
        return "Zdobadz kartę";
    }

    public override Sprite getIcon()
    {
        return Resources.Load<Sprite>("Icons/DrawCardAction");
    }
}