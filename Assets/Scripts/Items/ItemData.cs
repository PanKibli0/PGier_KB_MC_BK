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
    public List<BaseAction> actions = new List<BaseAction>();

    public void Use(Unit user, Unit selectedTarget = null)
    {
        if (user == null) return;

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

            desc += action.getCardDescription(source, target, true) + "\n";
        }

        return desc;
    }
}