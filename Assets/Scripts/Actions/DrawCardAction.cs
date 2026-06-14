using UnityEngine;

[System.Serializable]
public class DrawCardAction : BaseAction
{
    public int amount;

    public override void execute(Unit target, Unit source)
    {
        ActionEventBus.requestDrawCards(amount);
    }

    public override string getCardDescription(Unit source = null, Unit target = null, bool applyEffects = false)
    {
        return $"Dobierz {amount} karty.";
    }

    // public override Sprite getIcon() { return Resources.Load<Sprite>("Icons/draw"); }
    public override string getIconPath() { return "Icons/obrona"; }
    public override string getValue() { return $"{amount}"; }
}