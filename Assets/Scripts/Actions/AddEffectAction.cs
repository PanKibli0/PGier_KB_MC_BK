using UnityEngine;

[System.Serializable]
public class AddEffectAction : BaseAction
{
    [SerializeReference]
    public BaseStatusEffect effectToAdd;

    public override void execute(Unit target, Unit source)
    {
        if (effectToAdd == null) return;

        BaseStatusEffect newEffect = effectToAdd.Clone();
        target.addEffect(newEffect);

    }

    public override string getCardDescription(Unit source, Unit target = null)
    {
        if (effectToAdd == null) return "";
        return effectToAdd.getActionDescription();
    }

    public override string getIconPath() { return effectToAdd != null ? effectToAdd.getIconPath() : ""; }
    public override string getValue() { return effectToAdd?.getMainText(); }
}