[System.Serializable]
public class EnergyRegenEffect : BaseStatusEffect
{
    public int turns;
    public int energyPerTurn;

    public EnergyRegenEffect()
    {
        effectName = "Regeneracja energii";
        isMergeable = true;
        isDebuff = false;
    }

    public override string getMainText() { return turns.ToString(); }
    public override string getIconPath() { return GameManager.Instance?.selectedCharacter?.energySpriteName ?? ""; }

    public override void onTurnStart(Unit owner)
    {
        ActionEventBus.requestEnergyChange(energyPerTurn);
        turns--;
        if (turns <= 0) owner.removeEffect(this);
    }

    public override bool merge(BaseStatusEffect other)
    {
        turns += ((EnergyRegenEffect)other).turns;
        return turns <= 0;
    }

    public override string getDescription()
    {
        string sprite = GameManager.Instance?.selectedCharacter?.energySpriteName ?? "";
        return $"Na początku tury zyskaj {energyPerTurn} <sprite name=\"{sprite}\"> energii. Pozostało {turns} tur.";
    }

    public override string getActionDescription()
    {
        string sprite = GameManager.Instance?.selectedCharacter?.energySpriteName ?? "";
        return $"Zyskaj {energyPerTurn} <sprite name=\"{sprite}\"> energii przez {turns} tur.";
    }
}