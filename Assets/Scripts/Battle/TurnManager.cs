using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public event Action OnTurnEnded;
    public int turnNumber = 1;

    private RelicManager relics;
    private EnergySystem energySystem;
    private CardPileSystem cardPileSystem;
    private HandSystem handSystem;
    private UnitsManager unitsManager;

    public void init(RelicManager relics, EnergySystem energySystem, CardPileSystem cardPileSystem, HandSystem handSystem, UnitsManager unitsManager)
    {
        this.relics = relics;
        this.energySystem = energySystem;
        this.cardPileSystem = cardPileSystem;
        this.handSystem = handSystem;
        this.unitsManager = unitsManager;
    }

    public void calculateAllIntents()
    {
        foreach (Unit enemy in unitsManager.getEnemies())
            calculateUnitIntent(enemy);
        foreach (Unit ally in unitsManager.getAllies())
            calculateUnitIntent(ally);
    }

    public void endTurn()
    {
        unitsManager.player.onEffectsTurnEnd();
        relics.onTurnEnd(unitsManager.player, turnNumber);

        foreach (Unit enemy in unitsManager.getEnemies())
            executeUnitTurn(enemy);

        foreach (Unit ally in unitsManager.getAllies())
            executeUnitTurn(ally);

        handSystem.discardAllCards();
        energySystem.refreshEnergy();

        int drawCount = Random.Range(3, 6);
        for (int i = 0; i < drawCount; i++)
            cardPileSystem.drawCard();

        calculateAllIntents();

        unitsManager.player?.resetBlock();
        turnNumber++;

        relics.onTurnStart(unitsManager.player, turnNumber);
        unitsManager.player?.onEffectsTurnStart();

        OnTurnEnded?.Invoke();
    }

    public void calculateUnitIntent(Unit unit)
    {
        if (unit == null)
            return;

        MoveState state = unitsManager.getMoveState(unit);
        if (state == null)
            return;

        state.currentTurn = turnNumber;

        List<UnitMove> mandatoryMoves = new List<UnitMove>();
        List<UnitMove> filterMoves = new List<UnitMove>();

        foreach (UnitMove move in unit.unitData.moves)
        {
            if (move == null) continue;
            if (!state.canUse(move)) continue;

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

        MoveState state = unitsManager.getMoveState(unit);

        unit.resetBlock();
        unit.onEffectsTurnStart();

        if (unit.nextMove == null)
        {
            if (state != null && unit.unitData.moves != null)
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