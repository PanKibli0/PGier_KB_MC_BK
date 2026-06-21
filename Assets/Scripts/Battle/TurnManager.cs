using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public event Action OnTurnEnded;
    public int turnNumber = 1;
    [SerializeField] private TMP_Text turnLabel;

    private RelicManager relics;
    private EnergySystem energySystem;
    private CardPileSystem cardPileSystem;
    private HandSystem handSystem;
    private UnitsManager unitsManager;

    private int baseDrawCount;
    private int drawCountBonus = 0;

    private bool enemyAttackSfxPlayed = false;
    public void init(RelicManager relics, EnergySystem energySystem, CardPileSystem cardPileSystem, HandSystem handSystem, UnitsManager unitsManager)
    {
        this.relics = relics;
        this.energySystem = energySystem;
        this.cardPileSystem = cardPileSystem;
        this.handSystem = handSystem;
        this.unitsManager = unitsManager;
        baseDrawCount = GameManager.Instance.selectedCharacter.baseDrawCount;
        ActionEventBus.OnDrawCountChanged += onDrawCountChanged;
        ActionEventBus.OnTakeTurn += onTakeTurnRequested;

        updateTurnLabel();
    }

    void OnDestroy()
    {
        ActionEventBus.OnDrawCountChanged -= onDrawCountChanged;
        ActionEventBus.OnTakeTurn -= onTakeTurnRequested;
    }

    private void onDrawCountChanged(int amount)
    {
        drawCountBonus += amount;
    }

    private void updateTurnLabel()
    {
        if (turnLabel != null)
            turnLabel.text = $"Zakończ {turnNumber} turę";
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
        enemyAttackSfxPlayed = false;
        unitsManager.player.onEffectsTurnEnd();
        if (unitsManager.player == null) return;

        relics.onTurnEnd(unitsManager.player, turnNumber);

        foreach (Unit enemy in unitsManager.getEnemies())
        {
            executeUnitTurn(enemy);
            if (unitsManager.player == null) return;
        }

        foreach (Unit ally in unitsManager.getAllies())
            executeUnitTurn(ally);

        handSystem.discardAllCards();
        energySystem.refreshEnergy();

        int drawCount = Mathf.Max(1, baseDrawCount + drawCountBonus);
        for (int i = 0; i < drawCount; i++)
            cardPileSystem.drawCard();

        calculateAllIntents();

        unitsManager.player?.resetBlock();
        turnNumber++;

        relics.onTurnStart(unitsManager.player, turnNumber);
        unitsManager.player?.onEffectsTurnStart();

        updateTurnLabel();
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
                    if (condition == null) continue;
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

        List<UnitMove> pool = mandatoryMoves.Count > 0 ? mandatoryMoves : filterMoves;

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

    public void executeUnitTurn(Unit unit, bool activateEffects = true)
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

        if (unit.unitType == UnitType.Enemy && !enemyAttackSfxPlayed)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.attackOther);
            enemyAttackSfxPlayed = true;
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
        if (activateEffects) unit.onEffectsTurnEnd();

        if (state != null)
            state.onTurnEnd(unit.unitData.moves);
    }

    private void onTakeTurnRequested(Unit unit)
    {
        executeUnitTurn(unit, false);
        calculateUnitIntent(unit);
    }
}