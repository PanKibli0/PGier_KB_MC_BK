using System;

public static class ActionEventBus
{
    public static event Action<int> OnDrawCards;
    public static event Action<int> OnEnergyChange;
    public static event Action<UnitData, UnitType> OnSummon;
    public static event Action<int> OnDrawCountChanged;
    public static event Action<Unit> OnTakeTurn;

    public static void requestDrawCards(int amount)
    {
        OnDrawCards?.Invoke(amount);
    }

    public static void requestEnergyChange(int amount)
    {
        OnEnergyChange?.Invoke(amount);
    }

    public static void requestSummon(UnitData data, UnitType type)
    {
        OnSummon?.Invoke(data, type);
    }

    public static void requestDrawCountChange(int amount)
    {
        OnDrawCountChanged?.Invoke(amount);
    }

    public static void requestTakeTurn(Unit unit)
    {
        OnTakeTurn?.Invoke(unit);
    }
}