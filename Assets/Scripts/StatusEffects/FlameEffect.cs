using UnityEngine;

[System.Serializable]
public class FlameEffect : BaseStatusEffect
{
    public int duration;
    public int damagePerTurn;

    public FlameEffect()
    {
        effectName = "P³omieñ";
        isMergeable = true;
        isDebuff = true;
    }

    public override void onTurnEnd(Unit owner)
    {
        if (owner == null) return;
        owner.takeDamage(damagePerTurn);
        duration--;

        if (damagePerTurn > 1)
            damagePerTurn = Mathf.Max(1, damagePerTurn - 1);

        if (duration <= 0 || damagePerTurn <= 0)
            owner.removeEffect(this);
    }

    public override bool merge(BaseStatusEffect other)
    {
        FlameEffect o = other as FlameEffect;
        if (o == null) return false;
        duration += o.duration;
        damagePerTurn += o.damagePerTurn;
        damagePerTurn = Mathf.Min(damagePerTurn, 999);
        return false;
    }

    public override string getMainText() { return $"<color=#ff7a18>{duration}</color>"; }
    public override string getIconPath() { return "Icons/plomien"; }

    public override string getDescription()
    {
        return $"Zadaje +{damagePerTurn} obra¿eñ. S³abnie co turê.";
    }

    public override string getActionDescription()
    {
        return $"Na³ó¿ P³omieñ ({duration}) <sprite name=\"plomien\">";
    }
}