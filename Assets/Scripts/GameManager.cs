using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

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

    [Header("Run Stats")]
    public int enemiesKilled;
    public int floorsCompleted;

    [Header("Relics")]
    public RelicManager relicManager;
    public RelicPool relicPool;

    [Header("Events")]
    public EventsPool eventsPool;
    [HideInInspector] public EventData currentEvent;

    [Header("Items")]
    public PlayerInventory playerInventory;
    public ItemPool itemPool;

    [Header("Save")]
    public SaveDatabase saveDatabase;

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

        relicManager = new RelicManager(new List<RelicData>());
        playerInventory = new PlayerInventory();
    }

    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
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
        SaveSystem.deleteSave();

        selectedCharacter = character;
        maxHealth = character.maxHealth;
        setHealth(character.maxHealth);

        gold = character.startGold;
        OnGoldChanged?.Invoke(gold);

        currentDeck = new List<CardData>();
        foreach (StartCardEntry entry in character.startCards)
            for (int i = 0; i < entry.amount; i++)
                currentDeck.Add(entry.data);

        relicManager = new RelicManager(new List<RelicData>());
        playerInventory = new PlayerInventory();
        pendingBattleEnemies = null;
        currentMapNode = null;
        currentEvent = null;

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
        if (gold < 0) gold = 0;
        OnGoldChanged?.Invoke(gold);
    }

    public void addEnemyKill() { enemiesKilled++; }
    public void addFloorCount() { floorsCompleted++; }

    public RelicData getRandomRelic()
    {
        if (relicPool == null)
            return null;

        return relicPool.GetRandomRelic();
    }

    
    private bool buildTesterEnabled = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MainBar.Instance != null)
                Destroy(MainBar.Instance.gameObject);

            SceneManager.LoadScene("MenuScene");
        }
        #region BUILD TESTER 
        bool ctrlAlt = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                     && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt));

        if (ctrlAlt && Input.GetKeyDown(KeyCode.Space))
        {
            buildTesterEnabled = !buildTesterEnabled;
            Debug.Log($"Build Tester {(buildTesterEnabled ? "Enabled" : "Disabled")}");
        }

        if (!buildTesterEnabled)
            return;

        bool ctrlShift = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                       && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));

        if (ctrlShift && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("BattleRewardScene", LoadSceneMode.Additive);

        if (ctrlShift && Input.GetKeyDown(KeyCode.S))
            SceneManager.LoadScene("EndScreenScene", LoadSceneMode.Additive);

        if (ctrlShift && Input.GetKeyDown(KeyCode.P))
            setHealth(currentHealth / 2);

        if (ctrlShift && Input.GetKeyDown(KeyCode.E))
            SceneManager.LoadScene("EventScene");

        if (ctrlShift && Input.GetKeyDown(KeyCode.N))
            SaveSystem.save(this);

        if (ctrlShift && Input.GetKeyDown(KeyCode.M))
        {
            if (SaveSystem.load(this))
                SceneManager.LoadScene("MapScene");
        }

        if (ctrlShift && Input.GetKeyDown(KeyCode.B))
            SaveSystem.deleteSave();
        #endregion
    }

}