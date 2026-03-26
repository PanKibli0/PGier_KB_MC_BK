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
        //Debug.Log($"<color=cyan>Stworzono kartę: {data.cardName} z kosztem {currentCost}</color>");
        actions = new List<BaseAction>(); // buff and future things

        if (data.actions == null) return;

        foreach (var action in data.actions)
        {
            actions.Add(action.Clone());
        }
    }
}
