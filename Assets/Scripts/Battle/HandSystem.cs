using UnityEngine;
using System;
using System.Collections.Generic;

public class HandSystem : MonoBehaviour
{
    public List<Card> hand = new List<Card>();
    public int maxHandSize = 10;

    public event Action<Card> OnCardAddedToHand;
    public event Action OnHandCleared;

    private CardPileSystem cardPile;

    public void init(CardPileSystem cardPile)
    {
        this.cardPile = cardPile;
        cardPile.OnCardDrawn += addCard;
    }

    void OnDestroy()
    {
        if (cardPile != null)
            cardPile.OnCardDrawn -= addCard;
    }

    public void addCard(Card card)
    {
        if (hand.Count >= maxHandSize)
        {
            cardPile.discardCard(card);
            return;
        }
        hand.Add(card);
        OnCardAddedToHand?.Invoke(card);
    }

    public void removeCard(Card card)
    {
        if (hand.Remove(card))
        {
            if (card.data.exhaust)
                cardPile.exhaustCard(card);
            else
                cardPile.discardCard(card);
        }
    }

    public void discardAllCards()
    {
        if (hand.Count == 0) return;

        foreach (Card card in hand)
            cardPile.discardCard(card);

        hand.Clear();
        OnHandCleared?.Invoke();
    }
}