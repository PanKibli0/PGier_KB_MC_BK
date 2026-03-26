using UnityEngine;
using System;

public class EnergySystem : MonoBehaviour
{
    public static EnergySystem Instance;

    private int maxEnergy = 3;
    private int currentMaxEnergy;
    private int currentEnergy;

    public event Action<int, int> OnEnergyChanged;


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
        OnEnergyChanged?.Invoke(currentEnergy, currentMaxEnergy);
    }


    public void setCurrentMaxEnergy(int newMax)
    {
        currentMaxEnergy = newMax;
        OnEnergyChanged?.Invoke(currentEnergy, currentMaxEnergy);
    }


    public void addMaxEnergy(int amount)
    {
        maxEnergy += amount;
        currentMaxEnergy += amount;
        OnEnergyChanged?.Invoke(currentEnergy, currentMaxEnergy);
    }

    // przemyslec jak w playCard to ma być
    public bool canAfford(int cost)
    {
        return currentEnergy >= cost;
    }


    public void spendEnergy(int amount)
    {
        currentEnergy -= amount;
        OnEnergyChanged?.Invoke(currentEnergy, currentMaxEnergy);
    }


    public void addEnergy(int amount)
    {
        currentEnergy += amount;
        OnEnergyChanged?.Invoke(currentEnergy, currentMaxEnergy);
    }


    public void refreshEnergy(int amount = -1)
    {
        if (amount >= 0) currentEnergy = amount;
        else currentEnergy = currentMaxEnergy;

        OnEnergyChanged?.Invoke(currentEnergy, currentMaxEnergy);
    }
}