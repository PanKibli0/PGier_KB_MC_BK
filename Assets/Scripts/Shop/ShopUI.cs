using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject shopCardPrefab;
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private Button removeButton;
    [SerializeField] private TMP_Text removeCostText;
    [SerializeField] private GameObject removePanel;
    [SerializeField] private ShopRemovePanel shopRemovePanel;

    public int removeCost = 50;

    private void Start()
    {
        updateRemoveButtonState();
        createShopCardsUI();
    }

    private void updateRemoveButtonState()
    {
        bool canAfford = GameManager.Instance.gold >= removeCost;
        removeButton.interactable = canAfford;
        removeCostText.text = $"{removeCost}";
        removeCostText.color = canAfford ? Color.yellow : Color.red;
    }

    private void createShopCardsUI()
    {

        List<CardData> shopCards = new List<CardData>();
        CardPool charPool = GameManager.Instance.selectedCharacter.cardPool;
        CardPool generalPool = GameManager.Instance.generalCardPool;

        shopCards.AddRange(getUniqueCards(charPool.cards, 5));
        shopCards.AddRange(getUniqueCards(generalPool.cards, 2));

        Debug.Log($"Shop cards ({shopCards.Count}): {string.Join(", ", shopCards)}");

        for (int i = 0; i < 5; i++)
        {
            float x = -730 + i * 365;
            createCard(shopCards[i], x, 250);
        }

        for (int i = 0; i < 2; i++)
        {
            float x = -730 + i * 365;
            createCard(shopCards[5 + i], x, -175);
        }
    }


    private void createCard(CardData card, float x, float y)
    {
        GameObject cardObj = Instantiate(shopCardPrefab, cardsContainer);
        cardObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        cardObj.GetComponent<ShopCardItem>().init(card);
    }

    private List<CardData> getUniqueCards(List<CardData> source, int count)
    {
        if (source.Count <= count)
            return new List<CardData>(source);

        List<CardData> result = new List<CardData>();

        while (result.Count < count)
        {
            CardData card = source[Random.Range(0, source.Count)];
            if (!result.Contains(card))
                result.Add(card);
        }

        return result;
    }

    public void onRemoveClick()
    {
        shopRemovePanel.show();
    }

    public void onContinueClick()
    {
        GameManager.Instance.currentMapNode.onComplete();
        SceneManager.LoadScene("MapScene");
    }

    public void onRemoveConfirmed()
    {
        removeCost += 25;
        updateRemoveButtonState();
        removePanel.SetActive(false);
    }
}