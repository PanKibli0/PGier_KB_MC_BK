using UnityEngine;

[System.Serializable]
public class HealAction : BaseAction
{
    public int healAmount;

    public override void execute(Unit target, Unit source)
    {
        target.heal(healAmount);
    }

    public override string getCardDescription(Unit source = null, Unit target = null, bool applyEffects = false)
    {
        return $"Ulecz {healAmount} punktów <sprite name=\"zdrowie\"> zdrowia.";
    }

    public override string getIconPath() { return "Icons/zdrowie"; }
    public override string getValue() { return $"{healAmount}"; }
}