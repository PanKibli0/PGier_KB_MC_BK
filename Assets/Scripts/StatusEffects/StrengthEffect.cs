using UnityEngine;

[System.Serializable]
public class StrengthEffect : BaseStatusEffect
{
    public int value;

    public StrengthEffect()
    {
        effectName = "Siła";
        isMergeable = true;
        isDebuff = false;
    }

    public override string getMainText() { return value.ToString(); }

    public override void onDealDamage(Unit owner, Unit target, ref int damage)
    {
        damage += value;
    }

    public override bool merge(BaseStatusEffect other)
    {
        value += ((StrengthEffect)other).value;
        return value == 0;
    }


    public override string getDescription()
    {
        if (value > 0)
            return $"Zadajesz <color=green>{value}</color> obrażeń więcej.";
        else
            return $"Zadajesz <color=red>{-value}</color> obrażeń mniej.";
    }

    public override string getActionDescription()
    {
        if (value > 0)
            return $"Nałóż Siłę ({value})";
        else
            return $"Usuń Siłę ({-value})";
    }
}
