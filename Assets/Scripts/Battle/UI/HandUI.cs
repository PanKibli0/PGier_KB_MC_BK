using UnityEngine;

public class HandUI : MonoBehaviour
{
    [SerializeField] private GameObject cardUIPrefab;
    [SerializeField] private Transform handParent;

    
    private EnergySystem energySystem;
    private CardPileSystem cardPileSystem;
    private HandSystem handSystem;
    private UnitsManager unitsManager;
    private RelicManager relics;

    public void init(EnergySystem energySystem, CardPileSystem cardPileSystem, HandSystem handSystem, UnitsManager unitsManager, RelicManager relics)
    {
        this.energySystem = energySystem;
        this.cardPileSystem = cardPileSystem;
        this.handSystem = handSystem;
        this.unitsManager = unitsManager;
        this.relics = relics;

        handSystem.OnCardAddedToHand += createCardUI;
        handSystem.OnHandCleared += clearHandUI;
    }

    void OnDestroy()
    {
        if (handSystem != null)
        {
            handSystem.OnCardAddedToHand -= createCardUI;
            handSystem.OnHandCleared -= clearHandUI;
        }
    }

    void createCardUI(Card card)
    {
        GameObject cardObj = Instantiate(cardUIPrefab, handParent);
        cardObj.GetComponent<CardUIPlayable>()
            .init(card, energySystem, cardPileSystem, handSystem, unitsManager, relics);
    }

    void clearHandUI()
    {
        foreach (Transform child in handParent)
            Destroy(child.gameObject);
    }
}