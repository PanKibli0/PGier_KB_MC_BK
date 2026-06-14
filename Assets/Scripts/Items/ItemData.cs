using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    [SerializeReference]
    public List<BaseAction> actions = new List<BaseAction>();

    public void use(Unit user, Unit selectedTarget = null)
    {
        if (user == null || actions == null || actions.Count == 0) return;

        List<Unit> targets = TargetingSystem.getTargets(user, actions[0].targetType, selectedTarget);

        foreach (Unit target in targets)
        {
            if (target == null) continue;
            foreach (var action in actions)
            {
                if (action == null) continue;
                action.execute(target, user);
            }
        }
    }

    public string getDescription(Unit source = null, Unit target = null)
    {
        string desc = "";
        foreach (var action in actions)
        {
            if (action == null) continue;
            desc += $"{action.getCardDescription(source, target, true)} {TargetingSystem.getTargetText(action.targetType)}\n";
        }
        return desc;
    }
}
