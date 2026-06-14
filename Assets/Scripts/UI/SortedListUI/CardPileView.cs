using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardPileView : BaseCardPanel
{
    [SerializeField] private Button sortOrderButton;

    private PileType currentPileType;
    private CardPileSystem cardPileSystem;

    public void setCardPileSystem(CardPileSystem cards)
    {
        cardPileSystem = cards;
    }

    public void open(PileType pileType)
    {
        if (cardPileSystem == null) return;

        currentPileType = pileType;
        gameObject.SetActive(true);
        loadCardsByPileType(pileType);

        if (sortOrderButton != null)
            sortOrderButton.interactable = (pileType != PileType.Draw);
    }

    public void openDraw() { open(PileType.Draw); }
    public void openDiscard() { open(PileType.Discard); }
    public void openExhaust() { open(PileType.Exhaust); }

    private void loadCardsByPileType(PileType pileType)
    {
        switch (pileType)
        {
            case PileType.Draw:
                currentCards = new List<Card>(cardPileSystem.drawPile);
                currentSortMode = CardSorter.SortMode.Alphabetical;
                break;
            case PileType.Discard:
                currentCards = new List<Card>(cardPileSystem.discardPile);
                currentSortMode = CardSorter.SortMode.Order;
                break;
            case PileType.Exhaust:
                currentCards = new List<Card>(cardPileSystem.exhaustPile);
                currentSortMode = CardSorter.SortMode.Order;
                break;
        }
        sortAscending = true;
        sortAndDisplay();
    }

    protected override List<Card> loadCards()
    {
        return currentCards;
    }

    protected override void onCardSelected(Card card) { }

    protected override void displayCards()
    {
        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);

        foreach (Card card in currentCards)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            CardUIBase cardUI = cardObj.GetComponent<CardUIBase>();
            cardUI.init(card);
        }
    }

    public void close()
    {
        gameObject.SetActive(false);
    }
}

public enum PileType
{
    Draw,
    Discard,
    Exhaust
}