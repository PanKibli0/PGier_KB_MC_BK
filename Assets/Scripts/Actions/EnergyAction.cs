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
        string sprite = GameManager.Instance?.selectedCharacter?.energySpriteName ?? "";
        if (amount > 0)
            return $"Zyskaj {amount} <sprite name=\"{sprite}\"> energii";
        else
            return $"Strac {(-amount)} <sprite name=\"{sprite}\"> energii";
    }


    public override string getIconPath()
    {
        return GameManager.Instance?.selectedCharacter?.energySpriteName ?? "";
    }
    public override string getValue() { return $"{amount}"; }
}