using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitsManager : MonoBehaviour
{
    public static UnitsManager Instance;
    [SerializeField] private UnitStatsUIManager statsUIManager;

    [SerializeField] private Transform allyContainer;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private GameObject unitPrefab;

    public Unit player;
    public List<Unit> allies = new List<Unit>();
    public List<Unit> enemies = new List<Unit>();

    public Vector3[] allySpawnPositions = new Vector3[3];
    public Vector3[] enemySpawnPositions = new Vector3[4];
    public Vector3 playerSpawnPosition;

    private bool[] allySlots = new bool[3];
    private bool[] enemySlots = new bool[4];
    private Dictionary<Unit, int> unitSlot = new Dictionary<Unit, int>();

    public event Action OnUnitsChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }

    private int getFreeAllySlot()
    {
        for (int i = 0; i < allySlots.Length; i++)
            if (!allySlots[i]) return i;
        return -1;
    }

    private int getFreeEnemySlot()
    {
        for (int i = 0; i < enemySlots.Length; i++)
            if (!enemySlots[i]) return i;
        return -1;
    }

    public bool spawn(UnitData data, UnitType type)
    {
        if (data == null || unitPrefab == null) return false;

        int slot = -1;
        if (type == UnitType.Ally) slot = getFreeAllySlot();
        else if (type == UnitType.Enemy) slot = getFreeEnemySlot();
        else return false;



        if (slot == -1) return false;

        Vector3 spawnPos = (type == UnitType.Ally) ? allySpawnPositions[slot] : enemySpawnPositions[slot];

        GameObject newUnitObj = Instantiate(unitPrefab);
        newUnitObj.transform.position = spawnPos;
        Unit newUnit = newUnitObj.GetComponent<Unit>();
        newUnit.init(data, type, statsUIManager);

        if (data.graphicPrefab != null)
        {
            GameObject graphic = Instantiate(data.graphicPrefab, newUnitObj.transform);
            graphic.transform.localPosition = Vector3.zero;
        }

        if (type == UnitType.Ally)
        {
            allySlots[slot] = true;
            allies.Add(newUnit);
            if (allyContainer != null)
                newUnitObj.transform.SetParent(allyContainer, false);
        }
        else
        {
            enemySlots[slot] = true;
            enemies.Add(newUnit);
            if (enemyContainer != null)
                newUnitObj.transform.SetParent(enemyContainer, false);
        }

        unitSlot[newUnit] = slot;
        OnUnitsChanged?.Invoke();

        return true;
    }

    public void spawnPlayer(CharacterData data)
    {
        if (data == null || unitPrefab == null) return;

        GameObject newUnitObj = Instantiate(unitPrefab, allyContainer);
        newUnitObj.transform.localPosition = playerSpawnPosition;

        if (data.graphicPrefab != null)
            Instantiate(data.graphicPrefab, newUnitObj.transform);

        Unit newUnit = newUnitObj.GetComponent<Unit>();
        newUnit.init(data, UnitType.Player, statsUIManager);

        if (GameManager.Instance != null)
        {
            newUnit.currentHealth = GameManager.Instance.currentHealth;
            newUnit.maxHealth = GameManager.Instance.maxHealth;
            newUnit.currentMaxHealth = GameManager.Instance.maxHealth;
        }

        player = newUnit;
    }

    public void addUnitAtSlot(Unit unit, int slot)
    {
        if (unit == null) return;
        if (unit.unitType == UnitType.Ally && slot >= 0 && slot < allySlots.Length && !allySlots[slot])
        {
            allySlots[slot] = true;
            allies.Add(unit);
            unit.transform.position = allySpawnPositions[slot];
            unitSlot[unit] = slot;
            OnUnitsChanged?.Invoke();
        }
        else if (unit.unitType == UnitType.Enemy && slot >= 0 && slot < enemySlots.Length && !enemySlots[slot])
        {
            enemySlots[slot] = true;
            enemies.Add(unit);
            unit.transform.position = enemySpawnPositions[slot];
            unitSlot[unit] = slot;
            OnUnitsChanged?.Invoke();
        }
    }

    public void removeUnit(Unit unit)
    {
        if (unit == null) return;

        Debug.Log("REMOVE UNIT: " + unit.unitName);

        if (unitSlot.TryGetValue(unit, out int slot))
        {
            if (unit.unitType == UnitType.Ally)
            {
                if (slot >= 0 && slot < allySlots.Length)
                    allySlots[slot] = false;

                allies.RemoveAll(u => u == unit);
            }
            else if (unit.unitType == UnitType.Enemy)
            {
                if (slot >= 0 && slot < enemySlots.Length)
                    enemySlots[slot] = false;

                enemies.RemoveAll(u => u == unit);
            }

            unitSlot.Remove(unit);
        }
        else
        {
            Debug.LogWarning("Not found in unitSlot: " + unit.unitName);

            allies.RemoveAll(u => u == unit);
            enemies.RemoveAll(u => u == unit);
        }

        OnUnitsChanged?.Invoke();
    }

    public bool canSummonAlly()
    {
        return getFreeAllySlot() != -1;
    }

    public bool canSummonEnemy()
    {
        return getFreeEnemySlot() != -1;
    }

    public List<Unit> getAllies()
    {
        return new List<Unit>(allies);
    }

    public List<Unit> getEnemies()
    {
        return new List<Unit>(enemies);
    }
 
    public void onUnitDied(Unit unit)
    {
        removeUnit(unit);
        checkCombatEnd();
    }
    private void checkCombatEnd()
    {
        bool playerAlive = player != null && player.currentHealth > 0;

        enemies.RemoveAll(e => e == null || e.currentHealth <= 0);

        bool enemiesAlive = enemies.Count > 0;

        Debug.Log("ENEMIES COUNT: " + enemies.Count);

        if (!playerAlive)
        {
            SceneManager.LoadScene("EndScreenScene", LoadSceneMode.Additive);
        }
        else if (!enemiesAlive)
        {
            GameManager.Instance.addFloorCount();
            SceneManager.LoadScene("BattleRewardScene", LoadSceneMode.Additive);
        }
    }
}