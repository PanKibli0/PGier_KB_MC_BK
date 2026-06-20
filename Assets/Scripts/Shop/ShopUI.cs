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

    [Header("Items")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemsContainer;

    public int removeCost = 50;

    private void Start()
    {
        updateRemoveButtonState();
        createShopCardsUI();
        createShopItemsUI();
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
        GameManager gm = GameManager.Instance;
        List<CardData> charCards = getUniqueCards(gm.selectedCharacter.cardPool.cards, 5);
        for (int i = 0; i < charCards.Count; i++)
            createCard(charCards[i], -730 + i * 365, 250);
    }

    private void createCard(CardData card, float x, float y)
    {
        GameObject cardObj = Instantiate(shopCardPrefab, cardsContainer);
        cardObj.GetComponent<ShopCardItem>().init(card);
    }

    private void createShopItemsUI()
    {
        GameManager gm = GameManager.Instance;

        List<ItemData> items = new List<ItemData>();

        while (items.Count < 3)
        {
            ItemData item = gm.itemPool.GetRandomItem();
            if (item != null && !items.Contains(item))
                items.Add(item);
        }

        for (int i = 0; i < items.Count; i++)
        {
            GameObject obj = Instantiate(itemPrefab, itemsContainer, false);

            obj.transform.SetAsLastSibling();

            var ui = obj.GetComponentInChildren<ShopItemUI>();
            ui.init(items[i]);
        }
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
        gameObject.SetActive(false);
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