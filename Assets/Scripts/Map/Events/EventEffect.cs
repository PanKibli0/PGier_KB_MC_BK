using System;
using UnityEngine;

[System.Serializable]
public class EventEffect
{
    public EventEffectType type;
    public int intValue;
    public RelicData relic;
}

public enum EventEffectType
{
    AddGold,
    HealPlayer,
    TakeDamage,
    AddRelic,
    StartBattle
}