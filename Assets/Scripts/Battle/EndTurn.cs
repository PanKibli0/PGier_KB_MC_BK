using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EndTurn : MonoBehaviour
{
    public event Action OnTurnEnded;


    public void endTurn()
    {
        UnitsManager.Instance.player.onEffectsTurnEnd();

        foreach (Unit enemy in UnitsManager.Instance.getEnemies())
            enemy?.takeTurn();

        foreach (Unit ally in UnitsManager.Instance.getAllies())
            ally?.takeTurn();

        if (HandSystem.Instance != null)
            HandSystem.Instance.discardAllCards();

        if (EnergySystem.Instance != null)
            EnergySystem.Instance.refreshEnergy();

        int drawCount = Random.Range(3, 6);
        for (int i = 0; i < drawCount; i++)
            if (CardPileSystem.Instance != null) CardPileSystem.Instance.drawCard();

        foreach (Unit enemy in UnitsManager.Instance.getEnemies())
            enemy?.calculateIntent();

        foreach (Unit ally in UnitsManager.Instance.getAllies())
            ally?.calculateIntent();

        UnitsManager.Instance.player?.onEffectsTurnStart();
        OnTurnEnded?.Invoke();
    }
}