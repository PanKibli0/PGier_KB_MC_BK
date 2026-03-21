using UnityEngine;
using System;
using System.Collections.Generic;

public class HandSystem : MonoBehaviour
{
    public CardUIManager CardUIManager;
    public CardPileSystem cardPileSystem;

    public List<Card> hand = new List<Card>();

    public event Action<Card> OnCardAddedToHand;
    public event Action OnHandCleared;

    void OnEnable()
    {
        if (cardPileSystem != null)
            cardPileSystem.OnCardDrawn += addCard;
    }

    void OnDisable()
    {
        if (cardPileSystem != null)
            cardPileSystem.OnCardDrawn -= addCard;
    }

    public void addCard(Card card)
    {
        hand.Add(card);

        OnCardAddedToHand?.Invoke(card);
    }


    public void discardAllCards()
    {
        if (hand.Count == 0) return;

        foreach (Card card in hand)
        {
            cardPileSystem.discardCard(card);
        }

        hand.Clear();
        OnHandCleared?.Invoke();
    }
}
