using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public event Action OnTurnEnded;
    public int turnNumber = 1;

    private void Start()
    {
        foreach (Unit enemy in UnitsManager.Instance.getEnemies())
            calculateUnitIntent(enemy);
        foreach (Unit ally in UnitsManager.Instance.getAllies())
            calculateUnitIntent(ally);
    }

    public void endTurn()
    {
        UnitsManager.Instance.player.onEffectsTurnEnd();

        foreach (Unit enemy in UnitsManager.Instance.getEnemies())
            executeUnitTurn(enemy);

        foreach (Unit ally in UnitsManager.Instance.getAllies())
            executeUnitTurn(ally);

        if (HandSystem.Instance != null)
            HandSystem.Instance.discardAllCards();

        if (EnergySystem.Instance != null)
            EnergySystem.Instance.refreshEnergy();

        int drawCount = Random.Range(3, 6);
        for (int i = 0; i < drawCount; i++)
            if (CardPileSystem.Instance != null) CardPileSystem.Instance.drawCard();

        foreach (Unit enemy in UnitsManager.Instance.getEnemies())
            calculateUnitIntent(enemy);

        foreach (Unit ally in UnitsManager.Instance.getAllies())
            calculateUnitIntent(ally);

        UnitsManager.Instance.player?.resetBlock();
        UnitsManager.Instance.player?.onEffectsTurnStart();

        turnNumber++;
        OnTurnEnded?.Invoke();
    }

    public void calculateUnitIntent(Unit unit)
    {
        if (unit == null)
            return;

        MoveState state = UnitsManager.Instance.getMoveState(unit);
        if (state == null)
            return;

        state.currentTurn = turnNumber;

        List<UnitMove> mandatoryMoves = new List<UnitMove>();
        List<UnitMove> filterMoves = new List<UnitMove>();

        foreach (UnitMove move in unit.unitData.moves)
        {
            if (!state.canUse(move))
                continue;

            bool isMandatory = false;
            if (move.conditions != null)
            {
                foreach (MoveCondition condition in move.conditions)
                {
                    if (condition.mandatory)
                    {
                        isMandatory = true;
                        break;
                    }
                }
            }

            if (isMandatory)
                mandatoryMoves.Add(move);
            else
                filterMoves.Add(move);
        }

        List<UnitMove> pool;

        if (mandatoryMoves.Count > 0)
            pool = mandatoryMoves;
        else
            pool = filterMoves;

        if (pool.Count == 0)
        {
            unit.hideIntent();
            return;
        }

        float totalWeight = 0f;
        foreach (UnitMove move in pool)
            totalWeight += move.weight;

        float roll = Random.Range(0f, totalWeight);
        float current = 0f;
        UnitMove chosen = pool[0];

        foreach (UnitMove move in pool)
        {
            current += move.weight;
            if (roll <= current)
            {
                chosen = move;
                break;
            }
        }

        unit.showIntent(chosen);
    }

    public void executeUnitTurn(Unit unit)
    {
        if (unit == null)
            return;

        MoveState state = UnitsManager.Instance.getMoveState(unit);

        unit.resetBlock();
        unit.onEffectsTurnStart();

        if (unit.nextMove == null)
        {
            if (state != null)
                state.onTurnEnd(unit.unitData.moves);
            unit.onEffectsTurnEnd();
            return;
        }

        foreach (var action in unit.nextMove.actions)
        {
            if (action == null) continue;

            List<Unit> targets = TargetingSystem.getTargets(unit, action.targetType);

            foreach (Unit target in targets) 
            { 
                if (target != null)
                    action.execute(target, unit);
            }
        }

        if (state != null)
            state.recordUse(unit.nextMove);

        unit.hideIntent();
        unit.onEffectsTurnEnd();

        if (state != null)
            state.onTurnEnd(unit.unitData.moves);
    }
}