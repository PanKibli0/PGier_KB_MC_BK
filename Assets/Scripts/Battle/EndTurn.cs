using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EndTurn : MonoBehaviour
{
    public event Action OnTurnEnded;

    public void endTurn()
    {
        // DEBUG
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            enemy.takeTurn();
        }
        // END DEBUG

        HandSystem.Instance.discardAllCards();

        int drawCount = Random.Range(3, 6);

        for (int i = 0; i < drawCount; i++)
            if (CardPileSystem.Instance != null) CardPileSystem.Instance.drawCard();

        OnTurnEnded?.Invoke();
    }
}
