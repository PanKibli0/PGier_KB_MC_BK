using UnityEngine;

[System.Serializable]
public class EnergyAction : BaseAction
{
    public int amount;

    public override void execute(Unit target, Unit source)
    {
        ActionEventBus.requestEnergyChange(amount);
    }

    public override string getCardDescription(Unit source = null, Unit target = null, bool applyEffects = false)
    {
        if (amount > 0)
            return $"Zyskaj {amount} <sprite name=\"obrona\"> energii";
        else
            return $"Strac {(-amount)} <sprite name=\"obrona\"> energii";
    }

    // public override Sprite getIcon() { return Resources.Load<Sprite>("Icons/energy"); }
    public override string getIconPath() { return "Icons/krwawienie"; }
    public override string getValue() { return $"{amount}"; }
}