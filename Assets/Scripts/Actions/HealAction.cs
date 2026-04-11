using UnityEngine;

[System.Serializable]
public class HealAction : BaseAction
{
    public int healAmount;

    public override void execute(Unit target, Unit source)
    {
        target.heal(healAmount);
    }

    public override string getCardDescription(Unit source, Unit target = null)
    {
        return $"Ulecz {healAmount} punktów zdrowia.";
    }

    // public override Sprite getIcon() { return Resources.Load<Sprite>("Icons/heal"); }
    public override string getValue() { return $"{healAmount}"; }
}