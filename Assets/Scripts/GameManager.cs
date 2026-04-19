using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CharacterData selectedCharacter;

    public int currentHealth;
    public int maxHealth;
    public List<CardData> currentDeck;

    public int gold;
    public event Action<int> OnGoldChanged;

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

    // DEBUG
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("BattleRewardScene", LoadSceneMode.Additive);
    }

    // END DEBUG
}