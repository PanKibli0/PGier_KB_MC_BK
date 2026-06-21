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
        GameManager gm = GameManager.Instance;
        CharacterData character = gm.selectedCharacter;

        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic(AudioManager.Instance.battleMusic);

        relics = gm.relicManager;
        energySystem = new EnergySystem(character.baseMaxEnergy);
        cardPileSystem = new CardPileSystem();

        handSystem.init(cardPileSystem);
        unitsManager.init(energySystem, cardPileSystem, handSystem, relics);
        unitsManager.startBattle();

        turnManager.init(relics, energySystem, cardPileSystem, handSystem, unitsManager);

        if (character != null)
            unitsManager.spawnPlayer(character);

        if (gm.pendingBattleEnemies != null)
        {
            foreach (var data in gm.pendingBattleEnemies)
                if (data != null)
                    unitsManager.spawn(data, UnitType.Enemy);
        }

        gm.pendingBattleEnemies = null;
        relics.onBattleStart(unitsManager.player);

        energyUI?.init(energySystem);
        handUI?.init(energySystem, cardPileSystem, handSystem, unitsManager, relics);
        cardPileUI?.init(cardPileSystem);
        cardPileView?.setCardPileSystem(cardPileSystem);

        cardPileSystem.setupDeck();

        turnManager.calculateAllIntents();

        MainBar.Instance?.setPlayerUnit(unitsManager.player);
    }

    void OnDestroy()
    {
        cardPileSystem?.cleanup();
        energySystem?.cleanup();
        MainBar.Instance?.setPlayerUnit(null);
    }
}
