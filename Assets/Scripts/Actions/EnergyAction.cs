using UnityEngine;

[System.Serializable]
public class EnergyAction : BaseAction
{
    public int amount;

    public override void execute(Unit target, Unit source)
    {
        if (EnergySystem.Instance == null) return;

        if (amount > 0)
            EnergySystem.Instance.addEnergy(amount);
        else if (amount < 0)
            EnergySystem.Instance.spendEnergy(-amount);
    }

    public override string getCardDescription(Unit source, Unit target = null)
    {
        if (amount > 0)
            return $"Zyskaj {amount} energii.";
        else
            return $"Strac {(-amount)} energii.";

    }
}