using UnityEngine;
using TMPro;

public class RemoveConfirmPanel : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private ShopRemovePanel removePanel;

    private Card cardToRemove;

    public void show(Card card)
    {
        cardToRemove = card;

        GameObject cardObj = Instantiate(cardPrefab, transform);
        cardObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
        cardObj.GetComponent<CardUIBase>().init(cardToRemove);

        costText.text = $"{shopUI.removeCost}";

        gameObject.SetActive(true);
    }

    public void onConfirm()
    {
        if (GameManager.Instance.gold >= shopUI.removeCost)
        {
            GameManager.Instance.spendGold(shopUI.removeCost);
            GameManager.Instance.currentDeck.Remove(cardToRemove.data);
            removePanel.refreshAfterRemoval();
        }

        gameObject.SetActive(false);
    }

    public void onCancel()
    {
        removePanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}