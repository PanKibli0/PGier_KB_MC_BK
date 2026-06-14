using UnityEngine;

[System.Serializable]
public class RemoveEffectAction : BaseAction
{
    [SerializeReference]
    public BaseStatusEffect effectToRemove;

    public override void execute(Unit target, Unit source)
    {
        if (effectToRemove == null) return;

        BaseStatusEffect toRemove = null;
        foreach (var effect in target.effects)
        {
            if (effect.GetType() == effectToRemove.GetType())
            {
                toRemove = effect;
                break;
            }
        }

        if (toRemove != null)
            target.removeEffect(toRemove);
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