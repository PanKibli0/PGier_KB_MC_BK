using UnityEngine;

[System.Serializable]
public class MultiplyEffectAction : BaseAction
{
    [SerializeReference]
    public BaseStatusEffect effectToMultiply;

    public override void execute(Unit target, Unit source)
    {
        if (effectToMultiply == null) return;

        foreach (var effect in target.effects)
        {
            if (effect.GetType() == effectToMultiply.GetType())
            {
                BaseStatusEffect clone = effect.Clone();
                effect.merge(clone);
                return;
            }
        }
    }

    public override string getCardDescription(Unit source = null, Unit target = null, bool applyEffects = false)
    {
        if (effectToMultiply == null) return "";
        return $"Podwój swoją <color=#FFD700>{effectToMultiply.effectName}</color>.";
    }

    public override string getIconPath() { return effectToMultiply != null ? effectToMultiply.getIconPath() : ""; }
    public override string getValue() { return "x2"; }
}