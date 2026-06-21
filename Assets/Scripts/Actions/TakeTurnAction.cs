using UnityEngine;

[System.Serializable]
public class TakeTurnAction : BaseAction
{

    public override void execute(Unit target, Unit source)
    {
        ActionEventBus.requestTakeTurn(target);
    }

    public override string getCardDescription(Unit source = null, Unit target = null, bool applyEffects = false)
    {
        return $"Wykonaj natychmiastowo akcje sojusznika.";
    }
    public override string getIconPath() { return "Icons/TakeTurn"; }
    public override string getValue() { return "<sprite name=\"TakeTurn\">"; }
}
