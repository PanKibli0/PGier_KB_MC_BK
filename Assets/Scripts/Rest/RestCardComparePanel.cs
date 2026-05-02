using UnityEngine;

public class RestCardComparePanel : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private RestSceneUI restSceneUI;
    [SerializeField] private GameObject upgradePanel;

    private Card originalCard;
    private Card upgradedCard;

    public void show(Card card)
    {
        originalCard = card;
        upgradedCard = new Card(card.data.upgrade);

        GameObject leftCard = Instantiate(cardPrefab, transform);
        leftCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-405, 100);
        leftCard.transform.localScale = Vector3.one * 1.5f;
        leftCard.GetComponent<CardUIBase>().init(originalCard);

        GameObject rightCard = Instantiate(cardPrefab, transform);
        rightCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(405, 100);
        rightCard.transform.localScale = Vector3.one * 1.5f;
        rightCard.GetComponent<CardUIBase>().init(upgradedCard);

        gameObject.SetActive(true);
    }

    public void onConfirm()
    {
        int index = GameManager.Instance.currentDeck.IndexOf(originalCard.data);
        if (index != -1)
            GameManager.Instance.currentDeck[index] = upgradedCard.data;

        restSceneUI.onUpgradeCompleted();
        gameObject.SetActive(false);
    }

    public void onCancel()
    {
        upgradePanel.SetActive(true);
        gameObject.SetActive(false);
    }
}