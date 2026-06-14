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

    public override void onTurnEnd(Unit owner)
    {
        ActionEventBus.requestDrawCountChange(-bonus);
        owner.removeEffect(this);
    }

    public override bool merge(BaseStatusEffect other)
    {
        DrawCountEffect o = (DrawCountEffect)other;
        ActionEventBus.requestDrawCountChange(o.bonus);
        bonus += o.bonus;
        return bonus == 0;
    }

    public override string getMainText() { return bonus > 0 ? $"+{bonus}" : $"{bonus}"; }
    public override string getIconPath() { return "Icons/DrawCardAction"; }

    public override string getDescription()
    {
        return bonus > 0
            ? $"W następnej turze dobierzesz o {bonus} więcej kart."
            : $"W następnej turze dobierzesz o {-bonus} mniej kart.";
    }

    public override string getActionDescription()
    {
        return bonus > 0
            ? $"W następnej turze dobierz +{bonus} kart <sprite name=\"DrawCardAction\">"
            : $"W następnej turze dobierz {bonus} kart <sprite name=\"DrawCardAction\">";
    }
}