using UnityEngine;

public class BattleSetup : MonoBehaviour
{
    [SerializeField] private UnitsManager unitsManager;
    [SerializeField] private HandSystem handSystem;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private EnergyUI energyUI;
    [SerializeField] private HandUI handUI;
    [SerializeField] private CardPileUI cardPileUI;
    [SerializeField] private CardPileView cardPileView;

    private EnergySystem energySystem;
    private CardPileSystem cardPileSystem;
    private RelicManager relics;

    void Start()
    {
        relics = GameManager.Instance.relicManager;

        energySystem = new EnergySystem(3);
        cardPileSystem = new CardPileSystem();

        handSystem.init(cardPileSystem);
        unitsManager.init(energySystem, cardPileSystem, handSystem, relics);
        turnManager.init(relics, energySystem, cardPileSystem, handSystem, unitsManager);

        CharacterData character = GameManager.Instance.selectedCharacter;
        if (character != null)
            unitsManager.spawnPlayer(character);

        UnitData[] enemies = GameManager.Instance.pendingBattleEnemies;
        if (enemies != null)
        {
            foreach (var data in enemies)
                if (data != null)
                    unitsManager.spawn(data, UnitType.Enemy);
        }

        GameManager.Instance.pendingBattleEnemies = null;
        GameManager.Instance.relicManager.onBattleStart(unitsManager.player);

        if (energyUI != null) energyUI.init(energySystem);
        if (handUI != null) handUI.init(energySystem, cardPileSystem, handSystem, unitsManager, relics);
        if (cardPileUI != null) cardPileUI.init(cardPileSystem);
        if (cardPileView != null) cardPileView.setCardPileSystem(cardPileSystem);

        cardPileSystem.setupDeck();

        turnManager.calculateAllIntents();
    }
}