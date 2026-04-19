using UnityEngine;

[System.Serializable]
public class BleedingEffect : BaseStatusEffect
{
    public int stacks;

    public BleedingEffect()
    {
        effectName = "Krwawienie";
        icon = Resources.Load<Sprite>("Icons/krwawienie");
        isMergeable = true;
        isDebuff = true;
    }

    public override string getMainText() { return stacks.ToString(); }

    public override void onTurnStart(Unit owner)
    {
        owner.takeDamage(stacks, DamageType.Pure);
        stacks--;
        if (stacks <= 0) owner.removeEffect(this);
    }

    public override bool merge(BaseStatusEffect other)
    {
        stacks += ((BleedingEffect)other).stacks;
        return stacks <= 0;
    }

    public override string getDescription()
    {
        return $"Na początek tury zadaje {stacks} obrażeń. Zmniejsza się o 1.";
    }

    public override string getActionDescription()
    {
        if (stacks > 0)
            return $"Nałóż Krwawienie ({stacks})";
        else
            return $"Usuń Krwawienie ({-stacks})";
    }
}