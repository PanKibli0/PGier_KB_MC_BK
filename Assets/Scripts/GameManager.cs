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

    public CardPool generalCardPool;

    [Header("Run Stats")]
    public int enemiesKilled;
    public int floorsCompleted;


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
    }

    public void setHealth(int value)
    {
        if (value >= maxHealth)
            value = maxHealth;
        if (value < 0)
            value = 0;

        currentHealth = value;

        OnHealthChanged?.Invoke();
    }

    public void startNewRun(CharacterData character)
    {
        selectedCharacter = character;
        maxHealth = character.maxHealth;
        setHealth(character.maxHealth);
        
        gold = character.startGold;
        currentDeck = new List<CardData>(character.startCards);

        MapData mapData = new MapData();
        mapData.nodes = MapGenerator.generateMap(enemyPool);
        currentMap = mapData;
        
        SceneManager.LoadScene("MapScene");
    }


    public void addGold(int amount)
    {
        gold += amount;
        Debug.Log($"<color=yellow>Zdobyto {amount} złota. Razem: {gold}</color>");
        OnGoldChanged?.Invoke(gold);
    }

    public void spendGold(int amount)
    {
        gold -= amount;
        OnGoldChanged?.Invoke(gold);
    }

    public void addEnemyKill()
    {
        enemiesKilled++;
    }
    public void addFloorCount()
    {
        floorsCompleted++;
    }


    // DEBUG
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("BattleRewardScene", LoadSceneMode.Additive);

        if (Input.GetKeyDown(KeyCode.S))
            SceneManager.LoadScene("EndScreenScene", LoadSceneMode.Additive);
    }

    // END DEBUG
}