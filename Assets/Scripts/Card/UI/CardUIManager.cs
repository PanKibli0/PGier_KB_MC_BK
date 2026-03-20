using UnityEngine;

public class CardUIManager : MonoBehaviour
{
    public CardUI cardUIPrefab;

    public void createCardUI(Card card, Transform parent)
    {
        if (card == null || parent == null || cardUIPrefab == null)
            return;

        CardUI cardUI = Instantiate(cardUIPrefab, parent);
        cardUI.init(card);
    }

}