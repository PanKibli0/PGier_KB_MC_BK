using UnityEngine;
using System;
using System.Collections.Generic;

public class HandSystem : MonoBehaviour
{
    public static HandSystem Instance;

    public List<Card> hand = new List<Card>();

    public event Action<Card> OnCardAddedToHand;
    public event Action OnHandCleared;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void OnEnable()
    {
        if (CardPileSystem.Instance != null)
            CardPileSystem.Instance.OnCardDrawn += addCard;
        
    }

    void OnDisable()
    {
        if (CardPileSystem.Instance != null)
            CardPileSystem.Instance.OnCardDrawn -= addCard;
    }

    public void addCard(Card card)
    {
        hand.Add(card);
        OnCardAddedToHand?.Invoke(card);
    }

    public void removeCard(Card card)
    {
        if (hand.Remove(card))
        {
            CardPileSystem.Instance.discardCard(card);
        }
    }

    public void discardAllCards()
    {
        if (hand.Count == 0) return;

        foreach (Card card in hand)
        {
            CardPileSystem.Instance.discardCard(card);
        }

        hand.Clear();
        OnHandCleared?.Invoke();
    }
}
