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
        return $"Otrzymaj {blockAmount} obrony.";
    }
}