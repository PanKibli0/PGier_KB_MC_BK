using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class CardPileSystem
{
    public List<Card> drawPile = new List<Card>();
    public List<Card> discardPile = new List<Card>();
    public List<Card> exhaustPile = new List<Card>();

    public event Action<Card> OnCardDrawn;
    public event Action<int> OnDrawPileChanged;
    public event Action<int> OnDiscardPileChanged;
    public event Action<int> OnExhaustPileChanged;


    public CardPileSystem()
    {
        drawPile.Clear();
        discardPile.Clear();
        exhaustPile.Clear();

        ActionEventBus.OnDrawCards += drawCards;

        if (GameManager.Instance != null && GameManager.Instance.currentDeck != null)
            foreach (var cardData in GameManager.Instance.currentDeck)
                drawPile.Add(new Card(cardData));

        shuffle(drawPile);
    }

    public void cleanup()
    {
        ActionEventBus.OnDrawCards -= drawCards;
    }

    public void setupDeck()
    {
        OnDrawPileChanged?.Invoke(drawPile.Count);
        OnDiscardPileChanged?.Invoke(discardPile.Count);
        OnExhaustPileChanged?.Invoke(exhaustPile.Count);

        int end = Random.Range(3, 6);
        drawCards(end);
    }

    void drawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
            drawCard();
    }

    void refillDrawPile()
    {
        drawPile = new List<Card>(discardPile);
        discardPile.Clear();

        shuffle(drawPile);

        OnDiscardPileChanged?.Invoke(discardPile.Count);
    }

    void shuffle(List<Card> pile)
    {
        for (int i = 0; i < pile.Count; i++)
        {
            int randomIndex = Random.Range(i, pile.Count);
            (pile[i], pile[randomIndex]) = (pile[randomIndex], pile[i]);
        }
    }

    public void drawCard()
    {
        if (drawPile.Count == 0 && discardPile.Count == 0) return;

        if (drawPile.Count == 0)
            refillDrawPile();

        Card drawnCard = drawPile[0];
        drawPile.RemoveAt(0);

        OnCardDrawn?.Invoke(drawnCard);
        OnDrawPileChanged?.Invoke(drawPile.Count);

    }



    public void discardCard(Card card)
    {
        if (card == null) return;
        discardPile.Add(card);
        OnDiscardPileChanged?.Invoke(discardPile.Count);
    }

    public void exhaustCard(Card card)
    {
        if (card == null) return;
        Debug.Log("EXHAUST ADD: " + card.data.cardName +
             " | total: " + exhaustPile.Count);
        exhaustPile.Add(card);
        OnExhaustPileChanged?.Invoke(exhaustPile.Count);
    }

}
