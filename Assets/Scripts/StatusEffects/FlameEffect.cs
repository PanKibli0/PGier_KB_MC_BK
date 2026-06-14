using UnityEngine;

[System.Serializable]
public class FlameEffect : BaseStatusEffect
{
    public int duration;
    public int value;

    public FlameEffect()
    {
        effectName = "P³omieñ";
        isMergeable = true;
        isDebuff = true;
    }

    public override void onDealDamage(Unit owner, Unit target, ref int damage)
    {
        damage += value * 2;
    }

    public override void onTurnEnd(Unit owner)
    {
        duration--;
        value /= 2;

        if (duration <= 0 || value <= 0)
            owner.removeEffect(this);
    }

    public override bool merge(BaseStatusEffect other)
    {
        FlameEffect o = (FlameEffect)other;

        duration += o.duration;
        value += o.value;

        return false;
    }

    public override string getMainText()
    {
        return $"<color=#ff7a18>{duration}</color>";
    }

    public override string getIconPath()
    {
        return "Icons/flame";
    }

    public override string getDescription()
    {
        return $"Zadaje +{value * 2} obra¿eñ. S³abnie co turê.";
    }

    public override string getActionDescription()
    {
        return $"Na³ó¿ P³omieñ ({duration}) <sprite name=\"flame\">";
    }
}