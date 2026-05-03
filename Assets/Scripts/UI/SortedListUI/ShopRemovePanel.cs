using System.Collections.Generic;
using UnityEngine;

public class ShopRemovePanel : BaseCardPanel
{
    [SerializeField] private RemoveConfirmPanel confirmPanel;
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private GameObject mainPanel;

    public void show()
    {
        refreshDisplay();
        gameObject.SetActive(true);
    }

    protected override List<Card> loadCards()
    {
        List<Card> cards = new List<Card>();
        foreach (CardData cardData in GameManager.Instance.currentDeck)
            cards.Add(new Card(cardData));
        return cards;
    }

    protected override void onCardSelected(Card card)
    {
        gameObject.SetActive(false);
        confirmPanel.show(card);
    }

    public void onBackButton()
    {
        mainPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void refreshAfterRemoval()
    {
        shopUI.onRemoveConfirmed();
        gameObject.SetActive(false);
    }
}