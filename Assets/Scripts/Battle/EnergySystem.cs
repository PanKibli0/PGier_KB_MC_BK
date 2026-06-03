using UnityEngine;
using System;

public class EnergySystem
{
    private int maxEnergy;
    public int currentMaxEnergy;
    public int currentEnergy;

    public event Action OnEnergyChanged;

    public EnergySystem(int startMaxEnergy = 3)
    {
        maxEnergy = startMaxEnergy;
        currentMaxEnergy = startMaxEnergy;
        currentEnergy = startMaxEnergy;

        ActionEventBus.OnEnergyChange += energyChange;
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

    public bool canAfford(int cost) { return currentEnergy >= cost; }


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

    private void energyChange(int amount)
    {
        if (amount > 0)
            addEnergy(amount);
        else
            spendEnergy(-amount);
    }


    public void refreshEnergy(int amount = -1)
    {
        if (amount >= 0) currentEnergy = amount;
        else currentEnergy = currentMaxEnergy;

        OnEnergyChanged?.Invoke();
    }
}