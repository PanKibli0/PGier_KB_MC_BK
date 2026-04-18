using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TargetingSystem
{
    public static List<Unit> getTargets(Unit source, TargetType targetType, Unit selectedTarget = null)
    {
        UnitType sourceType = source.unitType;

        switch (targetType)
        {
            case TargetType.Self:
                return new List<Unit> { source };

            case TargetType.SelectedEnemy:
                return getSelectedEnemy(sourceType, source, selectedTarget);

            case TargetType.SelectedAlly:
                return getSelectedAlly(sourceType, source, selectedTarget);

            case TargetType.RandomEnemy:
                return getRandom(getEnemies(sourceType));

            case TargetType.RandomAlly:
                return getRandom(getAllies(sourceType, source));

            case TargetType.RandomUnit:
                return getRandom(getAllUnits());

            case TargetType.AllEnemies:
                return getEnemies(sourceType);

            case TargetType.AllAllies:
                return getAllies(sourceType, source);

            case TargetType.AllUnits:
                return getAllUnits();

            default:
                return new List<Unit>();
        }
    }


    #region WYBOR CELU

    private static List<Unit> getSelectedEnemy(UnitType sourceType, Unit source, Unit selectedTarget)
    {
        List<Unit> enemies = getEnemies(sourceType);

        if (sourceType == UnitType.Player)
        {
            if (selectedTarget != null && enemies.Contains(selectedTarget))
                return new List<Unit> { selectedTarget };

            return new List<Unit>();
        }

        bool prioritizePlayer = (sourceType == UnitType.Enemy);
        Unit target = getLowestHealthUnit(enemies, prioritizePlayer);
        return new List<Unit> { target };
    }


    private static List<Unit> getSelectedAlly(UnitType sourceType, Unit source, Unit selectedTarget)
    {
        List<Unit> allies = getAllies(sourceType, source);

        if (sourceType == UnitType.Player)
        {
            if (selectedTarget != null && allies.Contains(selectedTarget))
                return new List<Unit> { selectedTarget };

            return new List<Unit>();
        }

        if (sourceType == UnitType.Ally)
        {
            Unit target;

            if (Random.Range(0, 100) < 25)
            {
                List<Unit> otherAllies = new List<Unit>(allies);
                otherAllies.Remove(UnitsManager.Instance.player);
                if (otherAllies.Count > 0)
                    target = getHighestHealthUnit(otherAllies);
                else
                    target = UnitsManager.Instance.player;
            }
            else
                target = UnitsManager.Instance.player;

            return new List<Unit> { target };
        }

        if (sourceType == UnitType.Enemy)
        {
            Unit enemyTarget;

            if (Random.Range(0, 100) < 25)
            {
                List<Unit> otherEnemies = new List<Unit>(allies);
                otherEnemies.Remove(source);
                if (otherEnemies.Count > 0)
                    enemyTarget = getHighestHealthUnit(otherEnemies);
                else
                    enemyTarget = source;
            }
            else
                enemyTarget = getHighestPriorityUnit(allies);

            return new List<Unit> { enemyTarget };
        }

        return new List<Unit>();
    }

    #endregion


    #region LISTY JEDNOSTEK

    private static List<Unit> getEnemies(UnitType sourceType)
    {
        if (sourceType == UnitType.Player || sourceType == UnitType.Ally)
            return UnitsManager.Instance.getEnemies();

        List<Unit> enemies = new List<Unit> { UnitsManager.Instance.player };
        enemies.AddRange(UnitsManager.Instance.getAllies());
        return enemies;
    }


    private static List<Unit> getAllies(UnitType sourceType, Unit source)
    {
        List<Unit> allies;

        if (sourceType == UnitType.Player)
            allies = UnitsManager.Instance.getAllies();
        else if (sourceType == UnitType.Ally)
        {
            allies = new List<Unit> { UnitsManager.Instance.player };
            allies.AddRange(UnitsManager.Instance.getAllies());
        }
        else
            allies = UnitsManager.Instance.getEnemies();

        if (!allies.Contains(source))
            allies.Add(source);

        return allies;
    }


    private static List<Unit> getAllUnits()
    {
        List<Unit> all = new List<Unit> { UnitsManager.Instance.player };
        all.AddRange(UnitsManager.Instance.getAllies());
        all.AddRange(UnitsManager.Instance.getEnemies());
        return all;
    }

    #endregion


    #region LOSOWY CEL

    private static List<Unit> getRandom(List<Unit> units)
    {
        if (units == null || units.Count == 0)
            return new List<Unit>();

        return new List<Unit> { units[Random.Range(0, units.Count)] };
    }

    #endregion


    #region POMOC

    private static Unit getLowestHealthUnit(List<Unit> units, bool prioritizePlayer)
    {
        if (units == null || units.Count == 0)
            return null;

        Unit lowest = units[0];

        foreach (Unit unit in units)
        {
            if (unit.currentHealth < lowest.currentHealth)
                lowest = unit;
            else if (unit.currentHealth == lowest.currentHealth)
            {
                if (prioritizePlayer && unit.unitType == UnitType.Player)
                    lowest = unit;
                else if (unit.maxHealth < lowest.maxHealth)
                    lowest = unit;
            }
        }

        return lowest;
    }

    private static Unit getHighestHealthUnit(List<Unit> units)
    {
        if (units == null || units.Count == 0)
            return null;

        Unit highest = units[0];

        foreach (Unit unit in units)
        {
            if (unit.currentHealth > highest.currentHealth)
                highest = unit;
            else if (unit.currentHealth == highest.currentHealth && unit.maxHealth > highest.maxHealth)
                highest = unit;
        }

        return highest;
    }

    private static Unit getHighestPriorityUnit(List<Unit> units)
    {
        if (units == null || units.Count == 0)
            return null;

        Unit highest = units[0];

        foreach (Unit unit in units)
        {
            int currentPriority = getAIPriority(unit);
            int highestPriority = getAIPriority(highest);

            if (currentPriority > highestPriority)
                highest = unit;
            else if (currentPriority == highestPriority && unit.currentHealth > highest.currentHealth)
                highest = unit;
        }

        return highest;
    }


    private static int getAIPriority(Unit unit)
    {
        if (unit.unitType != UnitType.Enemy)
            return 0;

        switch (unit.getAIType())
        {
            case UnitAIType.Boss: return 3;
            case UnitAIType.Elite: return 2;
            case UnitAIType.Normal: return 1;
            default: return 0;
        }
    }

    #endregion
}