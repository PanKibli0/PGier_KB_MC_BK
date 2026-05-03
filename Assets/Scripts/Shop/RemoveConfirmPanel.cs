using UnityEngine;
using TMPro;

public class RemoveConfirmPanel : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private ShopRemovePanel removePanel;
    [SerializeField] private GameObject mainPanel;

    private Card cardToRemove;

    public void show(Card card)
    {
        cardToRemove = card;

        GameObject cardObj = Instantiate(cardPrefab, transform);
        cardObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-180, 100);
        cardObj.transform.localScale = Vector3.one * 1.5f;
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
        mainPanel.SetActive(true);
    }

    public void onCancel()
    {
        gameObject.SetActive(false);
        removePanel.gameObject.SetActive(true);
    }
}