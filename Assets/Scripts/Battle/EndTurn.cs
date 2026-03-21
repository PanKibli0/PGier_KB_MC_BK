using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EndTurn : MonoBehaviour
{
    public event Action OnTurnEnded;

    public void endTurn()
    {
        HandSystem.Instance.discardAllCards();

        int drawCount = Random.Range(3, 6);

        for (int i = 0; i < drawCount; i++)
            if (CardPileSystem.Instance != null) CardPileSystem.Instance.drawCard();

        OnTurnEnded?.Invoke();
    }
}
