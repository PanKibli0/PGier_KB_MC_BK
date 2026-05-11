using UnityEngine;

[System.Serializable]
public class WeaknessEffect : BaseStatusEffect
{
    public int duration;
    public float multiplier = 0.75f;

    public WeaknessEffect()
    {
        effectName = "Os³abienie";
        isMergeable = true;
        isDebuff = true;
    }

    public override string getMainText()
    {
        return $"<color=#0fdb57>{duration}</color>";
    }

    public override string getIconPath()
    {
        return "Icons/wrazliwosc";
        //zmienic na odpowiednia ikone
    }

    public override void onDealDamage(Unit owner, Unit target, ref int damage)
    {
        damage = Mathf.RoundToInt(damage * multiplier);
    }

    public override void onTurnEnd(Unit owner)
    {
        duration--;

        if (duration <= 0)
            owner.removeEffect(this);
    }

    public override bool merge(BaseStatusEffect other)
    {
        duration += ((WeaknessEffect)other).duration;
        return duration <= 0;
    }

    public override string getDescription()
    {
        return $"Zadaje o {Mathf.RoundToInt((1 - multiplier) * 100)}% mniej obra¿eñ. Pozosta³o {duration} tur.";
    }

    public override string getActionDescription()
    {
        if (duration > 0)
            return $"Na³ó¿ Os³abienie ({duration}) <sprite name=\"oslabienie\">";
        else
            return $"Usuñ Os³abienie ({-duration})";
    }
}