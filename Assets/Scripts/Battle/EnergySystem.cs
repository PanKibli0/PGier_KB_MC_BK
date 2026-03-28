using UnityEngine;
using System;

public class EnergySystem : MonoBehaviour
{
    public static EnergySystem Instance;

    [SerializeField] private int maxEnergy = 3;
    public int currentMaxEnergy;
    public int currentEnergy;

    public event Action OnEnergyChanged;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    void Start()
    {
        resetForBattle();
    }


    public void resetForBattle()
    {
        currentMaxEnergy = maxEnergy;
        currentEnergy = currentMaxEnergy;
        OnEnergyChanged?.Invoke();
    }


    public void setCurrentMaxEnergy(int newMax)
    {
        currentMaxEnergy = newMax;
        OnEnergyChanged?.Invoke();
    }


    public void addMaxEnergy(int amount)
    {
        maxEnergy += amount;
        currentMaxEnergy += amount;
        OnEnergyChanged?.Invoke();
    }

    // przemyslec jak w playCard to ma być
    public bool canAfford(int cost)
    {
        return currentEnergy >= cost;
    }


    public void spendEnergy(int amount)
    {
        currentEnergy -= amount;
        OnEnergyChanged?.Invoke();
    }


    public void addEnergy(int amount)
    {
        currentEnergy += amount;
        OnEnergyChanged?.Invoke();
    }


    public void refreshEnergy(int amount = -1)
    {
        if (amount >= 0) currentEnergy = amount;
        else currentEnergy = currentMaxEnergy;

        OnEnergyChanged?.Invoke();
    }
}