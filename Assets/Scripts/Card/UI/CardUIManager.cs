using UnityEngine;

public class CardUIManager : MonoBehaviour
{
    [SerializeField] private CardUIBase cardUIPrefabStatic;
    [SerializeField] private CardUIPlayable cardUIPrefabPlayable;
    [SerializeField] private CardUIClickable cardUIPrefabClickable;

    public void createCardUI(Card card, Transform parent, CardUIType type)
    {
        CardUIBase cardUI = null;

        switch (type)
        {
            case CardUIType.Playable:
                cardUI = Instantiate(cardUIPrefabPlayable, parent);
                break;
            case CardUIType.Clickable:
                cardUI = Instantiate(cardUIPrefabClickable, parent);
                break;
            default:
                cardUI = Instantiate(cardUIPrefabStatic, parent);
                break;
        }

        cardUI.init(card);
    }
}

public enum CardUIType
{
    Static,
    Playable,
    Clickable
}