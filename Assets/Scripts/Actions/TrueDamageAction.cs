using UnityEngine;

[System.Serializable]
public class TrueDamageAction : BaseAction
{
    public int damageAmount;

    public override void execute(Unit target, Unit source)
    {
        int finalDamage = damageAmount;

        if (source != null)
        {
            foreach (var effect in source.effects)
                effect.onDealDamage(source, target, ref finalDamage);
        }

        target.takeDamage(finalDamage, DamageType.True, source);
    }

    public override string getCardDescription(Unit source, Unit target = null)
    {
        int finalDamage = damageAmount;

        if (source != null)
        {
            foreach (var effect in source.effects)
                effect.onDealDamage(source, target, ref finalDamage);
        }

        return $"Zadaj {finalDamage} prawdziwych <sprite name=\"kruchosc\"> obrażeń.";
    }

    // public override Sprite getIcon() { return Resources.Load<Sprite>("Icons/trueDamage"); }
    public override string getIconPath() { return "Icons/obrona"; }
    public override string getValue() { return $"{damageAmount}"; }
}