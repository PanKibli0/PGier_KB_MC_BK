using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CardPileSystem : MonoBehaviour
{
    public List<Card> drawPile = new List<Card>();
    public List<Card> discardPile = new List<Card>();

    public HandSystem handSystem;
    public CardUIManager cardUIManager;

    public TMP_Text drawPileCountText;
    public TMP_Text discardPileCountText;

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

        Debug.Log($"<color=blue>Initialized draw pile with {drawPile.Count} cards.</color>");

        int end = Random.Range(3, 6);
		//end = 1;
        for (int i = 0; i < end; i++) drawCard();
        Debug.Log("<color=blue>Initial hand drawn.</color>");
    }
    // END DEBUG


    public void drawCard()
    {
        if (drawPile.Count == 0)
        {
            drawPile = new List<Card>(discardPile);
            discardPile.Clear();
			updateDiscardPileText();
        }

        int index = Random.Range(0, drawPile.Count);
        Card drawnCard = drawPile[index];
        drawPile.RemoveAt(index);
        handSystem.addCard(drawnCard); 
        Debug.Log($"<color=yellow>Drew {drawnCard.data.cardName} from the draw pile. Remaining cards in draw pile: {drawPile.Count}</color>");
        updateDrawPileText();
        updateDiscardPileText();    

    }

    public void addToDiscard(Card card)
    {
        discardPile.Add(card);
        updateDiscardPileText();
    }


    public void updateDrawPileText()
    {
        if (drawPileCountText != null)
            drawPileCountText.text = $"{drawPile.Count}";
    }


    public void updateDiscardPileText()
    {
        if (discardPileCountText != null)
            discardPileCountText.text = $"{discardPile.Count}";
    }
}
