using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CardPlayedVulnerableEffect : BaseStatusEffect
{
    public int vulnerableDuration;

    public CardPlayedVulnerableEffect()
    {
        effectName = "Nietykalny";
        isMergeable = true;
        isDebuff = false;
    }

    public override void onCardPlayed(Unit owner)
    {
        List<Unit> targets = TargetingSystem.getTargets(owner, TargetType.RandomEnemy);
        foreach (Unit target in targets)
        {
            VulnerableEffect effect = new VulnerableEffect();
            effect.duration = vulnerableDuration;
            target.addEffect(effect);
        }
    }

    public override bool merge(BaseStatusEffect other)
    {
        vulnerableDuration += ((CardPlayedVulnerableEffect)other).vulnerableDuration;
        return false;
    }

    public override string getMainText() { return vulnerableDuration.ToString(); }
    public override string getSecondaryText() { return "<color=#FFD700>K</color>"; }
    public override string getIconPath() { return "Icons/wrazliwosc"; }

    public override string getDescription()
    {
        return $"Za każdą zagraną kartę nałóż {vulnerableDuration} <sprite name=\"wrazliwosc\"> Wrażliwość na losowego wroga.";
    }

    public override string getActionDescription()
    {
        return $"Nałóż Nietykalnego ({vulnerableDuration}) <sprite name=\"wrazliwosc\">";
    }
}