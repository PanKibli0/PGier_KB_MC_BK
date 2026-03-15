using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Cards/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    public Sprite cardImage;
    public int cost;
    public string description;

    public CardAction[] actions;

    public CardData[] upgrades;

}

public enum CardType
{
    Attack,
    Skill,
    Power
}