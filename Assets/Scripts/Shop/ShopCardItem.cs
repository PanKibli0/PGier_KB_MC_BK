using UnityEngine;

public class ShopCardItem : MonoBehaviour
{
    [SerializeField] private CardUIClickable cardUI;
    [SerializeField] private PriceTag priceTag;

    private CardData cardData;

    public void init(CardData data)
    {
        cardData = data;
        cardUI.init(new Card(cardData));
        priceTag.setPrice(cardData.shopPrice);
    }

    private void Start()
    {
        cardUI.OnClick += onBuyClick;
    }

    private void OnDestroy()
    {
        cardUI.OnClick -= onBuyClick;
    }

    private void onBuyClick(Card clickedCard)
    {
        if (GameManager.Instance.gold >= cardData.shopPrice)
        {
            GameManager.Instance.spendGold(cardData.shopPrice);
            GameManager.Instance.currentDeck.Add(cardData);
            Destroy(gameObject);
        }
    }
}