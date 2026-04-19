using UnityEngine;

[System.Serializable]
public class ThornsEffect : BaseStatusEffect
{
    public int value;

    public ThornsEffect()
    {
        effectName = "Ciernie";
        isMergeable = true;
        isDebuff = false;
    }

    public override string getMainText() { return value.ToString(); }
    public override string getIconPath() { return "Icons/ciernie"; }


    public override void onReceiveDamage(Unit owner, Unit source, ref int damage)
    {
        if (source != null)
            source.takeDamage(value);
    }

    public override bool merge(BaseStatusEffect other)
    {
        value += ((ThornsEffect)other).value;
        return value == 0;
    }

    public override string getDescription()
    {
        return $"Zadaje {value} <sprite name=\"ciernie\"> obrażeń z powrotem atakującemu.";
    }

    public override string getActionDescription()
    {
        if (value > 0)
            return $"Nałóż Ciernie ({value}) <sprite name=\"ciernie\">";
        else
            return $"Usuń Ciernie ({-value}) <sprite name=\"ciernie\">";
    }
}