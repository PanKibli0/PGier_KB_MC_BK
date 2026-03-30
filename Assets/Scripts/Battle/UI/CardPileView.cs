using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardPileViewer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform gridContainer;
    [SerializeField] private CardUI cardUIPrefab;
    [SerializeField] private Button sortOrderButton;

    private List<Card> currentCards = new List<Card>();
    private PileType currentPileType;
    private SortType currentSortType = SortType.Alphabetical;
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
                currentSortType = SortType.Alphabetical;
                break;
            case PileType.Discard:
                currentCards.AddRange(CardPileSystem.Instance.discardPile);
                currentSortType = SortType.Order;
                break;
            case PileType.Exhaust:
                currentCards.AddRange(CardPileSystem.Instance.exhaustPile);
                currentSortType = SortType.Order;
                break;
        }

        sortAscending = true;
        sortAndDisplay();
    }

    private void displayCards(List<Card> cards)
    {
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Card card in cards)
        {
            CardUI cardUI = Instantiate(cardUIPrefab, gridContainer);
            cardUI.init(card);
            cardUI.enabled = false;
        }
    }

    private void sortAndDisplay()
    {
        List<Card> sorted = new List<Card>(currentCards);

        switch (currentSortType)
        {
            case SortType.Order:
                if (sortAscending)
                    sorted.Sort((a, b) => currentCards.IndexOf(a).CompareTo(currentCards.IndexOf(b)));
                else
                    sorted.Sort((a, b) => currentCards.IndexOf(b).CompareTo(currentCards.IndexOf(a)));
                break;

            case SortType.Type:
                if (sortAscending)
                    sorted.Sort((a, b) => a.data.type.CompareTo(b.data.type));
                else
                    sorted.Sort((a, b) => b.data.type.CompareTo(a.data.type));
                break;

            case SortType.Cost:
                if (sortAscending)
                    sorted.Sort((a, b) => a.currentCost.CompareTo(b.currentCost));
                else
                    sorted.Sort((a, b) => b.currentCost.CompareTo(a.currentCost));
                break;

            case SortType.Alphabetical:
                if (sortAscending)
                    sorted.Sort((a, b) => string.Compare(a.data.cardName, b.data.cardName));
                else
                    sorted.Sort((a, b) => string.Compare(b.data.cardName, a.data.cardName));
                break;
        }

        displayCards(sorted);
    }


    
    public void sortOrder()
    {
        if (currentPileType == PileType.Draw) return;

        if (currentSortType == SortType.Order)
            sortAscending = !sortAscending;
        else
        {
            currentSortType = SortType.Order;
            sortAscending = true;
        }
        sortAndDisplay();
    }


    public void sortType()
    {
        if (currentSortType == SortType.Type)
            sortAscending = !sortAscending;
        else
        {
            currentSortType = SortType.Type;
            sortAscending = true;
        }
        sortAndDisplay();
    }


    public void sortCost()
    {
        if (currentSortType == SortType.Cost)
            sortAscending = !sortAscending;
        else
        {
            currentSortType = SortType.Cost;
            sortAscending = true;
        }
        sortAndDisplay();
    }


    public void sortAlphabetical()
    {
        if (currentSortType == SortType.Alphabetical)
            sortAscending = !sortAscending;
        else
        {
            currentSortType = SortType.Alphabetical;
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


public enum SortType
{
    Order,
    Type,
    Cost,
    Alphabetical
}