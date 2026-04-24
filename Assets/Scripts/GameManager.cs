using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CharacterData selectedCharacter;

    public int currentHealth;
    public int maxHealth;
    public List<CardData> currentDeck;

    public int gold;
    public event Action<int> OnGoldChanged;

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
    }


    public void startNewRun(CharacterData character)
    {
        selectedCharacter = character;
        currentHealth = character.maxHealth;
        maxHealth = character.maxHealth;
        gold = character.startGold;
        currentDeck = new List<CardData>(character.startCards);
        enemiesKilled = 0;
        floorsCompleted = 0;
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