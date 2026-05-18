using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRelic", menuName = "Relic/RelicData")]
public class RelicData : ScriptableObject
{
    public string relicName;
    public Sprite icon;

    public RelicTrigger trigger;

    [SerializeReference]
    public List<BaseAction> actions;

    [Header("Optional conditions")]
    public int turnsBetweenTriggers;
    public CardType requiredCardType;
    public bool anyCardType;


}

public enum RelicTrigger
{
    OnBattleStart,
    OnBattleEnd,
    OnTurnStart,
    OnTurnEnd,
    OnCardPlayed,
    OnDamageDealt,
    OnDamageTaken
}