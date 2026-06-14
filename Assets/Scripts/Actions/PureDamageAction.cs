using UnityEngine;

[System.Serializable]
public class PureDamageAction : BaseAction
{
    public int damageAmount;

    public override void execute(Unit target, Unit source)
    {
        target.takeDamage(damageAmount, DamageType.Pure, source);
    }

    public override string getCardDescription(Unit source = null, Unit target = null, bool applyEffects = false)
    {
        return $"Zadaj {damageAmount} czystych <sprite name=\"Pure\"> obrażeń.";
    }

    public override string getIconPath() { return "Icons/Pure"; }
    public override string getValue() { return $"{damageAmount}"; }
}