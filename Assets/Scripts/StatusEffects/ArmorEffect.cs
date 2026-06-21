using UnityEngine;

[System.Serializable]
public class ArmorEffect : BaseStatusEffect
{
    public int blockPerTurn;

    public ArmorEffect()
    {
        effectName = "Zbroja";
        isMergeable = true;
        isDebuff = false;
    }

    public override void onTurnStart(Unit owner)
    {
        owner.addBlock(blockPerTurn);
    }

    public override bool merge(BaseStatusEffect other)
    {
        blockPerTurn += ((ArmorEffect)other).blockPerTurn;
        return false;
    }

    public override string getMainText() { return blockPerTurn.ToString(); }
    public override string getSecondaryText() { return ""; }
    public override string getIconPath() { return "Icons/barykada"; }

    public override string getDescription()
    {
        return $"Na początku każdej tury otrzymaj {blockPerTurn} <sprite name=\"obrona\"> obrony.";
    }

    public override string getActionDescription()
    {
        return $"Nałóż Zbroję ({blockPerTurn}) <sprite name=\"barykada\">";
    }
}