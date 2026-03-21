using UnityEngine;
using System.Collections.Generic;

public class Card
{
    public CardData data;
    public int currentCost;
    public List<BaseAction> actions; 

    public Card(CardData data)
    {
        this.data = data;
        currentCost = data.cost;
        actions = new List<BaseAction>(); // buff and future things

        if (data.actions == null) return;

        foreach (var action in data.actions)
        {
            actions.Add(action.Clone());
        }
    }
}
