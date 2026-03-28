using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardData", menuName = "CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public int cost;
    public CardType type;
    public Sprite image;

    [SerializeReference]
    public List<BaseAction> actions;

    public CardData upgrade;

}

public enum CardType
{
    Attack,
    Skill,
    Power,
    Curse
}