using UnityEngine;

[System.Serializable]
public class DodgeEffect : BaseStatusEffect
{
    public int charges;

    public DodgeEffect()
    {
        effectName = "Unik";
        isMergeable = true;
        isDebuff = false;
    }

    public override void onReceiveDamage(Unit owner, Unit source, ref int damage)
    {
        if (damage <= 0)
            return;
    }

    public void tryDodge(Unit owner, ref int damage, DamageType type)
    {
        if (type != DamageType.Normal)
            return;

        if (damage <= 0)
            return;

        damage = 0;
        charges--;

        if (charges <= 0)
            owner.removeEffect(this);
    }

    public override bool merge(BaseStatusEffect other)
    {
        DodgeEffect o = (DodgeEffect)other;
        charges += o.charges;
        return false;
    }

    public override string getMainText()
    {
        return charges.ToString();
    }

    public override string getIconPath()
    {
        return "Icons/dodge";
    }

    public override string getDescription()
    {
        return $"Neguje nastêpne {charges} ataki.";
    }
}