using UnityEngine;

[System.Serializable]
public class PoisonEffect : BaseStatusEffect
{
    // DEBUG
    public int stacks;

    public PoisonEffect()
    {
        effectName = "Trucizna";
        iconPath = "energia_mag";
        //isMergeable = true;
        isMergeable = false;
        isDebuff = true;
    }

    public override string getMainText() { return stacks.ToString(); }
    //public override string getSecondaryText() { return (10 * stacks).ToString(); }

    public override void onTurnEnd(Unit owner)
    {
        owner.takeDamage(stacks, DamageType.Pure);
        stacks--;
        if (stacks <= 0) owner.removeEffect(this);
    }

    public override bool merge(BaseStatusEffect other)
    {
        stacks += ((PoisonEffect)other).stacks;
        return stacks <= 0;
    }



    public override string getDescription()
    {
        return $"Na koniec tury zadaje {stacks} obrażeń. Zmniejsza się o 1.";
        //return $"Na koniec tury zadaje {stacks} <sprite name=\"damage\">obrażeń. Zmniejsza się o 1.";

    }

    public override string getActionDescription()
    {
        if (stacks > 0)
            return $"Nałóż Truciznę ({stacks})";
        else
            return $"Usuń Truciznę ({-stacks})";
    }
}