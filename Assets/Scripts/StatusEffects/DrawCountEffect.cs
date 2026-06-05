using UnityEngine;

[System.Serializable]
public class DrawCountEffect : BaseStatusEffect
{
    public int bonus;

    public DrawCountEffect()
    {
        effectName = "Dobieranie";
        isMergeable = true;
        isDebuff = false;
    }

    public override void onApply(Unit owner)
    {
        ActionEventBus.requestDrawCountChange(bonus);
    }

    public override void onRemove(Unit owner)
    {
        ActionEventBus.requestDrawCountChange(-bonus);
    }

    public override bool merge(BaseStatusEffect other)
    {
        DrawCountEffect o = (DrawCountEffect)other;
        ActionEventBus.requestDrawCountChange(o.bonus);
        bonus += o.bonus;
        return bonus == 0;
    }

    public override string getMainText() { return bonus > 0 ? $"+{bonus}" : $"{bonus}"; }
    public override string getIconPath() { return "Icons/draw"; }

    public override string getDescription()
    {
        return bonus > 0
            ? $"Dobierasz o {bonus} więcej kart na początku tury."
            : $"Dobierasz o {-bonus} mniej kart na początku tury.";
    }

    public override string getActionDescription()
    {
        return bonus > 0
            ? $"Dobieraj +{bonus} kart <sprite name=\"draw\">"
            : $"Dobieraj {bonus} kart <sprite name=\"draw\">";
    }
}