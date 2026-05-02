using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardPileView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CardUIManager cardUIManager;
    [SerializeField] private Transform gridContainer;
    [SerializeField] private Button sortOrderButton;

    private List<Card> currentCards = new List<Card>();
    private PileType currentPileType;
    private CardSorter.SortMode currentSortMode = CardSorter.SortMode.Alphabetical;
    private bool sortAscending = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) close();
    }

    public void open(PileType pileType)
    {
        currentPileType = pileType;
        gameObject.SetActive(true);
        loadCards(pileType);

        if (sortOrderButton != null)
            sortOrderButton.interactable = (pileType != PileType.Draw);
    }

    public void openDraw() { open(PileType.Draw); }
    public void openDiscard() { open(PileType.Discard); }
    public void openExhaust() { open(PileType.Exhaust); }

    private void loadCards(PileType pileType)
    {
        currentCards.Clear();

        switch (pileType)
        {
            case PileType.Draw:
                currentCards.AddRange(CardPileSystem.Instance.drawPile);
                currentSortMode = CardSorter.SortMode.Alphabetical;
                break;
            case PileType.Discard:
                currentCards.AddRange(CardPileSystem.Instance.discardPile);
                currentSortMode = CardSorter.SortMode.Order;
                break;
            case PileType.Exhaust:
                currentCards.AddRange(CardPileSystem.Instance.exhaustPile);
                currentSortMode = CardSorter.SortMode.Order;
                break;
        }

        sortAscending = true;
        sortAndDisplay();
    }

    private void displayCards(List<Card> cards)
    {
        foreach (Transform child in gridContainer)
            Destroy(child.gameObject);

        foreach (Card card in cards)
            cardUIManager.createCardUI(card, gridContainer, CardUIType.Static);
    }

    private void sortAndDisplay()
    {
        List<Card> sorted = CardSorter.sortCards(currentCards, currentSortMode, sortAscending);
        displayCards(sorted);
    }

    public void sortOrder()
    {
        if (currentPileType == PileType.Draw) return;

        if (currentSortMode == CardSorter.SortMode.Order)
            sortAscending = !sortAscending;
        else
        {
            currentSortMode = CardSorter.SortMode.Order;
            sortAscending = true;
        }
        sortAndDisplay();
    }

    public void sortType()
    {
        if (currentSortMode == CardSorter.SortMode.Type)
            sortAscending = !sortAscending;
        else
        {
            currentSortMode = CardSorter.SortMode.Type;
            sortAscending = true;
        }
        sortAndDisplay();
    }

    public void sortCost()
    {
        if (currentSortMode == CardSorter.SortMode.Cost)
            sortAscending = !sortAscending;
        else
        {
            currentSortMode = CardSorter.SortMode.Cost;
            sortAscending = true;
        }
        sortAndDisplay();
    }

    public void sortAlphabetical()
    {
        if (currentSortMode == CardSorter.SortMode.Alphabetical)
            sortAscending = !sortAscending;
        else
        {
            currentSortMode = CardSorter.SortMode.Alphabetical;
            sortAscending = true;
        }
        sortAndDisplay();
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