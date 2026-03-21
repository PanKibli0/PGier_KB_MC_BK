using UnityEngine;

public class HandUI : MonoBehaviour
{
    
    public CardUIManager cardUIManager;
    public Transform handParent;

    void OnEnable()
    {
        if (HandSystem.Instance != null)
        {
            HandSystem.Instance.OnCardAddedToHand += createCardUI;
            HandSystem.Instance.OnHandCleared += clearHandUI;
        }
    }

    void OnDisable()
    {
        if (HandSystem.Instance != null)
        {
            HandSystem.Instance.OnCardAddedToHand -= createCardUI;
            HandSystem.Instance.OnHandCleared -= clearHandUI;
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