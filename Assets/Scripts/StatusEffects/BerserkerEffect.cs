using UnityEngine;

[System.Serializable]
public class BerserkerEffect : BaseStatusEffect
{
    public int damagePerTurn;
    public int strengthPerTurn;

    public BerserkerEffect()
    {
        effectName = "Forma Berserka";
        isMergeable = true;
        isDebuff = false;
    }

    public override void onTurnStart(Unit owner)
    {
        owner.takeDamage(damagePerTurn, DamageType.Pure);

        StrengthEffect strength = new StrengthEffect();
        strength.value = strengthPerTurn;
        owner.addEffect(strength);
    }

    public override bool merge(BaseStatusEffect other)
    {
        BerserkerEffect o = (BerserkerEffect)other;
        damagePerTurn += o.damagePerTurn;
        strengthPerTurn += o.strengthPerTurn;
        return false;
    }

    public override string getMainText() { return strengthPerTurn.ToString(); }
    public override string getSecondaryText() { return damagePerTurn.ToString(); }
    public override string getIconPath() { return "Icons/Plomien"; }

    public override string getDescription()
    {
        return $"Na początku tury strać {damagePerTurn} <sprite name=\"atak\"> zdrowia i zyskaj {strengthPerTurn} <sprite name=\"sila\"> Siły.";
    }

    public override string getActionDescription()
    {
        return $"Nałóż Formę Berserka ({strengthPerTurn}) <sprite name=\"sila\">";
    }
}