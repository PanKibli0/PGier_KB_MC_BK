using UnityEngine;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private ItemUIClickable itemUI;
    [SerializeField] private PriceTag priceTag;

    private ItemData itemData;

    public void init(ItemData data)
    {
        itemData = data;

        itemUI.init(data);
        priceTag.setPrice(data.price);
    }

    private void Start()
    {
        itemUI.OnClick += onBuyClick;
    }

    private void OnDestroy()
    {
        itemUI.OnClick -= onBuyClick;
    }

    private void onBuyClick(ItemData clickedItem)
    {
        if (clickedItem == null) return;

        var gm = GameManager.Instance;
        var inventory = gm.playerInventory;

        if (gm.gold < clickedItem.price)
            return;
        if (!inventory.addItem(clickedItem))
        {
            Debug.Log("Cannot buy item - inventory full");
            return;
        }
        gm.spendGold(clickedItem.price);
        Destroy(transform.parent.gameObject);
    }
}