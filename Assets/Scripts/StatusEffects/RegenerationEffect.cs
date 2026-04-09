using UnityEngine;

[System.Serializable]
public class RegenerationEffect : BaseStatusEffect
{
    public int stacks;

    public RegenerationEffect()
    {
        effectName = "Regeneracja";
        isMergeable = true;
        isDebuff = false;
    }

    public override string getMainText() { return stacks.ToString(); }

    public override void onTurnStart(Unit owner)
    {
        owner.heal(stacks);
        stacks--;
        if (stacks <= 0) owner.removeEffect(this);
    }

    public override bool merge(BaseStatusEffect other)
    {
        stacks += ((RegenerationEffect)other).stacks;
        return stacks <= 0;
    }

    public override string getDescription()
    {
        return $"Na początek tury leczy {stacks} HP. Zmniejsza się o 1.";
    }
}