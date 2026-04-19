using UnityEngine;

[System.Serializable]
public class VulnerableEffect : BaseStatusEffect
{
    public int duration;
    public float multiplier = 1.5f;

    public VulnerableEffect()
    {
        effectName = "Wrażliwość";
        isMergeable = true;
        isDebuff = true;
    }

    public override string getMainText() { return duration.ToString(); }
    public override string getIconPath() { return "Icons/wrazliwosc"; }

    public override void onReceiveDamage(Unit owner, Unit source, ref int damage)
    {
        damage = Mathf.RoundToInt(damage * multiplier);
    }

    public override void onTurnEnd(Unit owner)
    {
        duration--;
        if (duration <= 0) owner.removeEffect(this);
    }

    public override bool merge(BaseStatusEffect other)
    {
        duration += ((VulnerableEffect)other).duration;
        return duration <= 0;
    }

    public override string getDescription()
    {
        return $"Otrzymuje {Mathf.RoundToInt((multiplier - 1) * 100)}% więcej obrażeń. Pozostało {duration} tur.";
    }

    public override string getActionDescription()
    {
        if (duration > 0)
            return $"Nałóż Wrażliwość ({duration})";
        else
            return $"Usuń Wrażliwość ({-duration})";
    }
}