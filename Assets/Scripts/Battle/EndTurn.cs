using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EndTurn : MonoBehaviour
{
    public HandSystem handSystem;
    public CardPileSystem cardPileSystem;

    public event Action OnTurnEnded;

    public void endTurn()
    {
        if (handSystem) handSystem.discardAllCards();

        int drawCount = Random.Range(3, 6);

        for (int i = 0; i < drawCount; i++)
            if (cardPileSystem) cardPileSystem.drawCard();

        OnTurnEnded?.Invoke();
    }
}
