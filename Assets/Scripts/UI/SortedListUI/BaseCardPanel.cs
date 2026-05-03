using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCardPanel : MonoBehaviour
{
    [SerializeField] protected Transform cardContainer;
    [SerializeField] protected GameObject cardPrefab;

    protected List<Card> currentCards = new List<Card>();
    protected CardSorter.SortMode currentSortMode = CardSorter.SortMode.Order;
    protected bool sortAscending = true;

    protected abstract List<Card> loadCards();
    protected abstract void onCardSelected(Card card);
    protected virtual void displayCards()
    {
        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);

        foreach (Card card in currentCards)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            CardUIClickable cardUI = cardObj.GetComponent<CardUIClickable>();
            cardUI.init(card);
            cardUI.OnClick += onCardSelected;
        }
    }

    protected void sortAndDisplay()
    {
        currentCards = CardSorter.sortCards(currentCards, currentSortMode, sortAscending);
        displayCards();
    }

    public void refreshDisplay()
    {
        currentCards = loadCards();
        sortAndDisplay();
    }

    public void sortOrder()
    {
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
}