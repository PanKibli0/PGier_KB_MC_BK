using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CardPlayedDamageEffect : BaseStatusEffect
{
    public int damageAmount;

    public CardPlayedDamageEffect()
    {
        effectName = "Armagedon";
        isMergeable = true;
        isDebuff = false;
    }

    public override void onCardPlayed(Unit owner)
    {
        List<Unit> targets = TargetingSystem.getTargets(owner, TargetType.AllEnemies);
        foreach (Unit target in targets)
            target.takeDamage(damageAmount, DamageType.Pure, owner);
    }

    public override bool merge(BaseStatusEffect other)
    {
        damageAmount += ((CardPlayedDamageEffect)other).damageAmount;
        return false;
    }

    public override string getMainText() { return damageAmount.ToString(); }
    public override string getSecondaryText() { return "<color=#FF4444>A</color>"; }
    public override string getIconPath() { return "Icons/atak"; }

    public override string getDescription()
    {
        return $"Za każdą zagraną kartę zadaj {damageAmount} <sprite name=\"atak\"> obrażeń wszystkim wrogom.";
    }

    public override string getActionDescription()
    {
        return $"Nałóż Armagedon ({damageAmount}) <sprite name=\"atak\">";
    }
}