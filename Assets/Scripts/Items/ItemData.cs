using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;

    public TargetType targetType;

    [SerializeReference]
    public List<BaseStatusEffect> effects = new List<BaseStatusEffect>();

    public void Use(Unit user, Unit selectedTarget = null)
    {
        if (user == null) return;

        List<Unit> targets = TargetingSystem.getTargets(user, targetType, selectedTarget);

        foreach (Unit target in targets)
        {
            if (target == null) continue;

            foreach (var effect in effects)
            {
                if (effect == null) continue;

                target.addEffect(effect.Clone());
            }
        }
    }
}