using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestCardUpgradePanel : MonoBehaviour
{
    [SerializeField] private Transform cardContainer;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Button backButton;
    [SerializeField] private RestCardComparePanel comparePanel;

    private List<Card> upgradableCards = new List<Card>();
    private CardSorter.SortMode currentSortMode = CardSorter.SortMode.Order;
    private bool sortAscending = true;
    private RestSceneUI parentUI;

    public void show(RestSceneUI parent)
    {
        parentUI = parent;
        loadUpgradableCards();
        sortAndDisplay();
    }

    private void loadUpgradableCards()
    {
        upgradableCards.Clear();
        foreach (CardData cardData in GameManager.Instance.currentDeck)
            if (cardData.upgrade != null)
                upgradableCards.Add(new Card(cardData));
        sortAscending = true;
        currentSortMode = CardSorter.SortMode.Order;
    }

    private void sortAndDisplay()
    {
        upgradableCards = CardSorter.sortCards(upgradableCards, currentSortMode, sortAscending);
        displayCards();
    }

    private void displayCards()
    {
        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);

        foreach (Card card in upgradableCards)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            CardUIClickable cardUI = cardObj.GetComponent<CardUIClickable>();
            cardUI.init(card);
            cardUI.OnClick += onCardSelected;
        }
    }

    private void onCardSelected(Card card)
    {
        comparePanel.show(card);
        gameObject.SetActive(false);
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

    public void onBackButton()
    {
        parentUI.onUpgradeCancelled();
    }
}