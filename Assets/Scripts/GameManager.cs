using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CharacterData selectedCharacter;

    public int currentHealth;
    public int maxHealth;
    public List<CardData> currentDeck;

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
        currentDeck = new List<CardData>(character.startCards);
    }
}