using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static event Action OnHealthChanged;

    public CharacterData selectedCharacter;

    [Header("Player Stats")]
    public int currentHealth;
    public int maxHealth;
    public List<CardData> currentDeck;

    [Header("Currency")]
    public int gold;
    public static event Action<int> OnGoldChanged;

    [Header("Map")]
    public EnemyPool enemyPool;
    public MapData currentMap;
    public BaseNode currentMapNode;

    public UnitData[] pendingBattleEnemies;
    public BattleDifficulty pendingBattleDifficulty;

    public CardPool generalCardPool;

    [Header("Run Stats")]
    public int enemiesKilled;
    public int floorsCompleted;

    [Header("Relics")]
    public RelicManager relicManager;
    public RelicData[] relicPool;


    [HideInInspector] public EventData currentEvent;
    public EventData testEvent;

    public PlayerInventory playerInventory;

    // DEBUG
    public List<ItemData> testItems;
    // END DEBUG

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        OnGoldChanged?.Invoke(gold);

        relicManager = new RelicManager(new List<RelicData>(relicPool));
        playerInventory = new PlayerInventory();

        // DEBUG
        foreach (var item in testItems)
            playerInventory.addItem(item);
        // END DEBUG
    }

    public void setHealth(int value)
    {
        if (value >= maxHealth) value = maxHealth;
        if (value < 0) value = 0;
        currentHealth = value;
        OnHealthChanged?.Invoke();
    }

    public void startNewRun(CharacterData character)
    {
        selectedCharacter = character;
        maxHealth = character.maxHealth;
        setHealth(character.maxHealth);

        gold = character.startGold;
        currentDeck = new List<CardData>();

        foreach (StartCardEntry entry in character.startCards)
            for (int i = 0; i < entry.amount; i++)
                currentDeck.Add(entry.data);

        MapData mapData = new MapData();
        mapData.nodes = new MapGenerator().generateMap(enemyPool);
        currentMap = mapData;

        enemiesKilled = 0;
        floorsCompleted = 0;

        SceneManager.LoadScene("MapScene");
    }

    public void addGold(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
    }

    public void spendGold(int amount)
    {
        gold -= amount;
        OnGoldChanged?.Invoke(gold);
    }

    public void addEnemyKill() { enemiesKilled++; }
    public void addFloorCount() { floorsCompleted++; }

    public RelicData getRandomRelic()
    {
        if (relicPool == null || relicPool.Length == 0) return null;
        return relicPool[Random.Range(0, relicPool.Length)];
    }

    // DEBUG
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("BattleRewardScene", LoadSceneMode.Additive);

        if (Input.GetKeyDown(KeyCode.S))
            SceneManager.LoadScene("EndScreenScene", LoadSceneMode.Additive);

        if (Input.GetKeyDown(KeyCode.P))
            setHealth(currentHealth / 2);

        if (Input.GetKeyDown(KeyCode.E))
            SceneManager.LoadScene("EventScene");
    }
    // END DEBUG
}