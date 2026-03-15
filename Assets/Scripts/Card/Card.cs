using UnityEngine;

public class Card
{
    public CardData data;
    public int currentCost;

    public Card(CardData data)
    {
        this.data = data;
        currentCost = data.cost;
    }
}
