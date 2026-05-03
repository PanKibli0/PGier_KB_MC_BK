using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestCardUpgradePanel : BaseCardPanel
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Button backButton;
    [SerializeField] private RestCardComparePanel comparePanel;
    [SerializeField] private RestSceneUI restSceneUI;

    public void show()
    {
        refreshDisplay();
        gameObject.SetActive(true);
    }

    protected override List<Card> loadCards()
    {
        List<Card> cards = new List<Card>();
        foreach (CardData cardData in GameManager.Instance.currentDeck)
            if (cardData.upgrade != null)
                cards.Add(new Card(cardData));
        return cards;
    }

    protected override void onCardSelected(Card card)
    {
        comparePanel.show(card);
        gameObject.SetActive(false);
    }

    public void onBackButton()
    {
        restSceneUI.onUpgradeCancelled();
    }
}