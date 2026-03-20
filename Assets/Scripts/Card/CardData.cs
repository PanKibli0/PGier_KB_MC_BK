using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Cards/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public CardType type;
    public Sprite image;
    public int cost;
    //public string description;

    public CardAction[] actions;

    public CardData upgrade;

}

public enum CardType
{
    Attack,
    Skill,
    Power,
    Status,
    Curse
}