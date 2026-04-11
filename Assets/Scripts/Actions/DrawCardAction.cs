using UnityEngine;

[System.Serializable]
public class DrawCardAction : BaseAction
{
    public int amount;

    public override void execute(Unit target, Unit source)
    {
        for (int i = 0; i < amount; i++)
        {
            if (CardPileSystem.Instance != null)
                CardPileSystem.Instance.drawCard();
        }
    }

    public override string getCardDescription(Unit source, Unit target = null)
    {
        return $"Dobierz {amount} karty.";
    }
}