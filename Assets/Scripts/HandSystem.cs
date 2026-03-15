using UnityEngine;
using System.Collections.Generic;

public class HandSystem : MonoBehaviour
{
    public Transform handUI;
    public CardUIManager CardUIManager;
    public CardPileSystem cardPileSystem;

    public List<Card> hand = new List<Card>();

    public void addCard(Card card)
    {
        hand.Add(card);

        if (CardUIManager && handUI) CardUIManager.createCardUI(card, handUI);

        Debug.Log($"<color=green>Added {card.data.name} to hand. Total cards in hand: {hand.Count}</color>");
    }


    public void discardAllCards()
    {
        if (hand.Count == 0) return;

        foreach (Card card in hand)
        {
            cardPileSystem.addToDiscard(card);
            Debug.Log($"<color=red>Discarded {card.data.name} from hand to discard pile.</color>");
        }
        hand.Clear();

        foreach (Transform cardUI in handUI)
        {
            Destroy(cardUI.gameObject);
        }

        Debug.Log($"<color=red>All cards discarded. Hand is now empty.</color>");
    }
}
