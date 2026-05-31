using UnityEngine;

[System.Serializable]
public class FlameEffect : BaseStatusEffect
{
    public int value;
    public int multiplier = 1;

    public FlameEffect()
    {
        effectName = "P³omieñ";
        isMergeable = true;
        isDebuff = true;
    }

    public override void onDealDamage(Unit owner, Unit target, ref int damage)
    {
        damage += value * multiplier;
    }

    public override void onTurnEnd(Unit owner)
    {
        value -= 2;

        if (value <= 0)
            owner.removeEffect(this);
    }

    public override bool merge(BaseStatusEffect other)
    {
        FlameEffect o = (FlameEffect)other;

        value += o.value;
        multiplier++;

        return false;
    }

    public override string getMainText()
    {
        return $"<color=#ff7a18>{value}</color>";
    }

    public override string getIconPath()
    {
        return "Icons/flame";
    }

    public override string getDescription()
    {
        return $"Zadaje +{value * multiplier} obra¿eñ. Si³a: {multiplier}. Traci 2 stacki na turê.";
    }

    public override string getActionDescription()
    {
        return $"Na³ó¿ {value} P³omien <sprite name=\"flame\">";
    }
}