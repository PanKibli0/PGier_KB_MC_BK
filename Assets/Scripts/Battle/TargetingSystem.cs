using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TargetingSystem
{
    private static UnitsManager unitsManager;

    public static void registerUnitsManager(UnitsManager manager)
    {
        unitsManager = manager;
    }


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
                otherAllies.Remove(unitsManager.player);
                if (otherAllies.Count > 0)
                    target = getHighestHealthUnit(otherAllies);
                else
                    target = unitsManager.player;
            }
            else
                target = unitsManager.player;

            return new List<Unit> { target };
        }

        if (sourceType == UnitType.Enemy)
        {
            Unit enemyTarget;

            if (Random.Range(0, 100) < 25)
            {
                List<Unit> otherEnemies = new List<Unit>(allies);
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
            return unitsManager.getEnemies();

        List<Unit> enemies = new List<Unit> { unitsManager.player };
        enemies.AddRange(unitsManager.getAllies());
        return enemies;
    }


    private static List<Unit> getAllies(UnitType sourceType, Unit source)
    {
        List<Unit> allies;

        if (sourceType == UnitType.Player)
            allies = unitsManager.getAllies();
        else if (sourceType == UnitType.Ally)
        {
            allies = new List<Unit> { unitsManager.player };
            allies.AddRange(unitsManager.getAllies());
        }
        else
            allies = unitsManager.getEnemies();

        if (!allies.Contains(source))
            allies.Add(source);

        return allies;
    }


    private static List<Unit> getAllUnits()
    {
        List<Unit> all = new List<Unit> { unitsManager.player };
        all.AddRange(unitsManager.getAllies());
        all.AddRange(unitsManager.getEnemies());
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

    public static bool isValidTarget(Unit source, TargetType targetType, Unit target)
    {
        switch (targetType)
        {
            case TargetType.SelectedEnemy:
                return getEnemies(source.unitType).Contains(target);
            case TargetType.SelectedAlly:
                return getAllies(source.unitType, source).Contains(target);
            default:
                return false;
        }
    }

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

    #region TEXT
    public static string getTargetText(TargetType targetType)
    {
        switch (targetType)
        {
            case TargetType.Self: return "(na siebie)";
            case TargetType.SelectedEnemy: return "(na wybranego wroga)";
            case TargetType.SelectedAlly: return "(na wybranego sojusznika)";
            case TargetType.RandomEnemy: return "(na losowego wroga)";
            case TargetType.RandomAlly: return "(na losowego sojusznika)";
            case TargetType.RandomUnit: return "(na losową jednostkę)";
            case TargetType.AllEnemies: return "(na wszystkich wrogów)";
            case TargetType.AllAllies: return "(na wszystkich sojuszników)";
            case TargetType.AllUnits: return "(na wszystkie jednostki)";
            default: return "";
        }
    }
    #endregion
}