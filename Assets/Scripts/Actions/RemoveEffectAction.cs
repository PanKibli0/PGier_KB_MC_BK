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
}