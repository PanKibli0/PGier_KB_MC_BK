using UnityEngine;

public class HandUI : MonoBehaviour
{

    [SerializeField] private GameObject cardUIPrefab;
    [SerializeField] private Transform handParent;

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
        GameObject cardObj = Instantiate(cardUIPrefab, handParent);
        cardObj.GetComponent<CardUIPlayable>().init(card);
    }

    void clearHandUI()
    {
        foreach (Transform child in handParent)
        {
            Destroy(child.gameObject);
        }
    }
}