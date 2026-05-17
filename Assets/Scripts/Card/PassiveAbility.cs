using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PassiveAbility
{
    public PassiveTrigger trigger;

    [SerializeReference]
    public List<BaseAction> actions;

    public PassiveAbility()
    {
        actions = new List<BaseAction>();
    }
}

public enum PassiveTrigger
{
    TurnStart,
    TurnEnd,
    ReceiveDamage,
    CardPlayed
}