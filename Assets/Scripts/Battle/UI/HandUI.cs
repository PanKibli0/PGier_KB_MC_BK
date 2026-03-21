using UnityEngine;

public class HandUI : MonoBehaviour
{
    public HandSystem handSystem;
    public CardUIManager cardUIManager;
    public Transform handParent;

    void OnEnable()
    {
        if (handSystem != null)
        {
            handSystem.OnCardAddedToHand += createCardUI;
            handSystem.OnHandCleared += clearHandUI;
        }
    }

    void OnDisable()
    {
        if (handSystem != null)
        {
            handSystem.OnCardAddedToHand -= createCardUI;
            handSystem.OnHandCleared -= clearHandUI;
        }
    }

    void createCardUI(Card card)
    {
        cardUIManager.createCardUI(card, handParent);
    }

    void clearHandUI()
    {
        foreach (Transform child in handParent)
        {
            Destroy(child.gameObject);
        }
    }
}