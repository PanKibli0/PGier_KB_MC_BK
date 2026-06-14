using UnityEngine;

[System.Serializable]
public class HealthLostBlockEffect : BaseStatusEffect
{
    public int blockAmount;

    public HealthLostBlockEffect()
    {
        effectName = "Tarcza bólu";
        isMergeable = true;
        isDebuff = false;
    }

    public override void onHealthLost(Unit owner, int damageTaken)
    {
        owner.addBlock(blockAmount);
    }

    public override bool merge(BaseStatusEffect other)
    {
        blockAmount += ((HealthLostBlockEffect)other).blockAmount;
        return false;
    }

    public override string getMainText() { return blockAmount.ToString(); }
    public override string getSecondaryText() { return "<color=#FF4444>!</color>"; }
    public override string getIconPath() { return "Icons/obrona"; }

    public override string getDescription()
    {
        return $"Za każdym razem gdy stracisz zdrowie zyskaj {blockAmount} <sprite name=\"obrona\"> obrony.";
    }

    public override string getActionDescription()
    {
        return $"Nałóż Tarczę bólu ({blockAmount}) <sprite name=\"obrona\">";
    }
}