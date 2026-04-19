using UnityEngine;
using System.Collections.Generic;

public class CardRewardPanel : MonoBehaviour
{
    [SerializeField] private Transform cardContainer;
    [SerializeField] private CardUIClickable cardUIPrefab;
    [SerializeField] private GameObject rewardsList;

    private CardReward cardReward;

    public void setCards(List<CardData> cards, CardReward cardReward)
    {
        this.cardReward = cardReward;

        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);

        foreach (var cardData in cards)
        {
            CardUIClickable cardUI = Instantiate(cardUIPrefab, cardContainer);
            cardUI.transform.localScale = Vector3.one * 1.5f;
            cardUI.init(new Card(cardData));
            cardUI.OnClick += onCardSelected;
        }
    }

    private void onCardSelected(Card card)
    {
        GameManager.Instance.currentDeck.Add(card.data);
        cardReward.complete();
        onCloseButtonClick();
    }

    public void onCloseButtonClick()
    {
        rewardsList.SetActive(true);
        gameObject.SetActive(false);
    }
}