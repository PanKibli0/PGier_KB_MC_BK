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

        if (source.unitType == UnitType.Player)
            GameManager.Instance.relicManager.onDamageDealt(source, target);

        target.takeDamage(finalDamage, DamageType.Normal, source);
    }

    public override string getCardDescription(Unit source = null, Unit target = null, bool applyEffects = false)
    {
        int finalDamage = damageAmount;

        if (applyEffects)
        {
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
        }

        if (finalDamage < damageAmount)
            return $"Zadaj <color=red>{finalDamage}</color> <sprite name=\"atak\"> obrażeń.";
        else if (finalDamage > damageAmount)
            return $"Zadaj <color=#BFFF00>{finalDamage}</color> <sprite name=\"atak\"> obrażeń.";
        else
            return $"Zadaj {finalDamage} <sprite name=\"atak\"> obrażeń.";
    }

    public override string getIconPath() { return "Icons/atak"; }
    public override string getValue() { return $"{damageAmount}"; }
}