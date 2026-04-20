using UnityEngine;

[System.Serializable]
public class BlockAction : BaseAction
{
    public int blockAmount;

    public override void execute(Unit target, Unit source)
    {
        target.addBlock(blockAmount);
    }

    public override string getCardDescription(Unit source, Unit target = null)
    {
        return $"Otrzymaj {blockAmount} <sprite name=\"obrona\"> obrony.";
    }

    // public override Sprite getIcon() { return Resources.Load<Sprite>("Icons/block"); }
    public override string getIconPath() { return "Icons/obrona"; }
    public override string getValue() { return $"{blockAmount}"; }
}