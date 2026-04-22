using UnityEngine;

[System.Serializable]
public class RemoveEffectAction : BaseAction
{
    [SerializeReference]
    public BaseStatusEffect effectToRemove;

    public override void execute(Unit target, Unit source)
    {
        if (effectToRemove == null) return;

        foreach (var effect in target.effects)
        {
            if (effect.GetType() == effectToRemove.GetType())
            {
                target.removeEffect(effect);
                return;
            }
        }
    }

    public override string getCardDescription(Unit source, Unit target = null, bool applyEffects = false)
    {
        if (effectToRemove == null) return "";
        return $"Usuń {effectToRemove.effectName}.";
    }

    // public override Sprite getIcon() { return effectToRemove?.icon; }
    public override string getIconPath() { return "Icons/obrona"; }
    public override string getValue() { return "<color=red>X</color>"; }
}