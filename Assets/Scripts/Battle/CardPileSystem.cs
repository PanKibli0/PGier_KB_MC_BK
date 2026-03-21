using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class CardPileSystem : MonoBehaviour
{
    public List<Card> drawPile = new List<Card>();
    public List<Card> discardPile = new List<Card>();
    public List<Card> exhaustPile = new List<Card>();

    public HandSystem handSystem;
    public CardUIManager cardUIManager;

    public event Action<Card> OnCardDrawn;
    public event Action<int> OnDrawPileChanged;
    public event Action<int> OnDiscardPileChanged;
    public event Action<int> OnExhaustPileChanged;

    // DEBUG
    void Start()
    {
        drawPile.Clear();        
        for (int i = 1; i <= 10; i++)
        {
            CardData data = ScriptableObject.CreateInstance<CardData>();
            data.cardName = $"Card {i}";

            drawPile.Add(new Card(data));
        }

        OnDiscardPileChanged?.Invoke(discardPile.Count);
        OnDrawPileChanged?.Invoke(drawPile.Count);

        int end = Random.Range(3, 6);
        for (int i = 0; i < end; i++) drawCard();


    }
    // END DEBUG

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
        // if (exhaust) exhaustCard(card);
        if (card == null) return;
        discardPile.Add(card);
        OnDiscardPileChanged?.Invoke(discardPile.Count);
    }

    public void exhaustCard(Card card)
    {
        if (card == null) return;
        exhaustPile.Add(card);
        OnExhaustPileChanged?.Invoke(exhaustPile.Count);
    }

}
