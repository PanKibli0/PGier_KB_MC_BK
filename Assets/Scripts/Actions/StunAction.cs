using UnityEngine;

[System.Serializable]
public class StunAction : BaseAction
{
    public override void execute(Unit target, Unit source)
    {
        target.hideIntent();
    }

    public override string getCardDescription(Unit source = null, Unit target = null, bool applyEffects = false)
    {
        return $"Ogłusz <sprite name=\"ogluszenie\"> przeciwnika.";
    }

    public override string getIconPath() { return "Icons/Ogluszenie"; }
    public override string getValue() { return ""; }
}