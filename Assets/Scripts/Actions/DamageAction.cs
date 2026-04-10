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
}