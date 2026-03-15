using UnityEngine;

public class CardUIManager : MonoBehaviour
{
    public CardUI cardUIPrefab;

    public void createCardUI(Card card, Transform parent)
    {
        if (!parent || !cardUIPrefab) return;
        CardUI cardUI = Instantiate(cardUIPrefab, parent);
        cardUI.init(card);

    }

    public void removeCardUI(CardUI cardUI)
    {
        if (cardUI) Destroy(cardUI.gameObject);
    }


}
