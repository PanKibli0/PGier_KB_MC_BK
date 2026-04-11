using UnityEngine;

[System.Serializable]
public class DamageAction : BaseAction
{
    public int damageAmount;

    public override void execute(Unit target, Unit source)
    {
        int finalDamage = damageAmount;

        foreach (var effect in source.effects)
            effect.onDealDamage(source, target, ref finalDamage);

        target.takeDamage(finalDamage, DamageType.Normal, source);
    }

    public override string getCardDescription(Unit source, Unit target = null)
    {
        int finalDamage = damageAmount;

        if (source != null)
        {
            foreach (var effect in source.effects)
                effect.onDealDamage(source, target, ref finalDamage);
        }

        if (target != null)
        {
            foreach (var effect in target.effects)
                effect.onReceiveDamage(target, source, ref finalDamage);
        }

        if (finalDamage < damageAmount)
            return $"Zadaj <color=red>{finalDamage}</color> obrażeń.";
        else if (finalDamage > damageAmount)
            return $"Zadaj <color=#BFFF00>{finalDamage}</color> obrażeń.";
        else
            return $"Zadaj {finalDamage} obrażeń.";
    }

    // public override Sprite getIcon() { return Resources.Load<Sprite>("Icons/damage"); }
    public override string getValue() { return $"{damageAmount}"; }
}