using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum UnitType
{
    Player,
    Ally,
    Enemy
}

public enum DamageType
{
    Normal,
    True,
    Pure
}

public class Unit : MonoBehaviour
{
    public string unitName;
    public UnitType unitType;

    public int maxHealth;
    public int currentMaxHealth;
    public int currentHealth;
    public int block;

    [SerializeReference] public List<BaseStatusEffect> effects = new List<BaseStatusEffect>();
    [SerializeReference] private UnitData unitData;
    public UnitMove nextMove;

    private UnitStatsUI statsUI;

    public event Action OnEffectsChanged;

    private bool isDead = false; 


    public void init(BaseUnitData data, UnitType type, UnitStatsUIManager statsUIManager)
    {
        unitName = data.unitName;
        unitType = type;

        if (data.minHealth == 0) maxHealth = data.maxHealth;
        else maxHealth = Random.Range(data.minHealth, data.maxHealth);


        currentMaxHealth = maxHealth;
        currentHealth = maxHealth;

        foreach (var effect in data.startEffects)
            if (effect != null)
                addEffect(effect.Clone());

        if (data is UnitData unitData)
            this.unitData = unitData;

        statsUIManager.createStatsUI(this);

        if (unitType != UnitType.Player)
            calculateIntent();
    }


    public void setStatsUI(UnitStatsUI ui)
    {
        statsUI = ui;
    }


    public void calculateIntent()
    {
        if (unitData == null || unitData.moves == null || unitData.moves.Count == 0)
            return;

        int randomIndex = Random.Range(0, unitData.moves.Count);
        nextMove = unitData.moves[randomIndex];

        statsUI?.showIntent(nextMove);
    }


    public void clearIntent()
    {
        nextMove = null;
        statsUI?.hideIntent();
    }


    public void takeTurn()
    {
        if (unitType == UnitType.Player || isDead)
            return;

        resetBlock();
        onEffectsTurnStart();

        if (nextMove == null)
            return;

        foreach (var action in nextMove.actions)
        {
            if (action == null) continue;

            List<Unit> targets = TargetingSystem.getTargets(this, action.targetType);

            foreach (Unit target in targets)
                action.execute(target, this);
        }

        clearIntent();
        onEffectsTurnEnd();
    }


    public void takeDamage(int damage, DamageType type = DamageType.Normal, Unit source = null)
    {
        if (isDead) return;

        if (type != DamageType.Pure)
        {
            foreach (var effect in effects)
                effect.onReceiveDamage(this, source, ref damage);
        }

        if (type == DamageType.Normal && block > 0)
        {
            int blockUsed = Mathf.Min(block, damage);
            block -= blockUsed;
            damage -= blockUsed;
        }

        currentHealth -= damage;
        statsUI?.updateUI();

        if (currentHealth <= 0 && !isDead)
        {
            die();
        }

        if (unitType == UnitType.Player && GameManager.Instance != null)
        {
            GameManager.Instance.setHealth(currentHealth);
        }
    }
    public bool IsDead()
    {
        return isDead;
    }

    public void die()
    {
        isDead = true;

        UnitsManager.Instance.onUnitDied(this);

        Debug.Log("UNIT DIED: " + unitName);

        Destroy(gameObject);
    }



    public void addBlock(int amount)
    {
        block += amount;
        statsUI?.updateUI();
    }


    public void heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;

        if (currentHealth > currentMaxHealth)
            currentHealth = currentMaxHealth;

        statsUI?.updateUI();

        if (unitType == UnitType.Player && GameManager.Instance != null)
        {
            GameManager.Instance.setHealth(currentHealth);
        }
    }


    public void resetBlock()
    {
        block = 0;
        statsUI?.updateUI();
    }


    public void addEffect(BaseStatusEffect newEffect)
    {
        if (newEffect.isMergeable)
        {
            foreach (var existing in effects)
            {
                if (existing.GetType() == newEffect.GetType())
                {
                    if (existing.merge(newEffect))
                        effects.Remove(existing);
                    else
                        existing.onApply(this);

                    OnEffectsChanged?.Invoke();
                    return;
                }
            }
        }

        effects.Add(newEffect);
        newEffect.onApply(this);
        OnEffectsChanged?.Invoke();
    }


    public void removeEffect(BaseStatusEffect effect)
    {
        effects.Remove(effect);
        OnEffectsChanged?.Invoke();
    }


    public void onEffectsTurnStart()
    {
        for (int i = effects.Count - 1; i >= 0; i--)
            effects[i].onTurnStart(this);
        OnEffectsChanged?.Invoke();
    }

    public void onEffectsTurnEnd()
    {
        for (int i = effects.Count - 1; i >= 0; i--)
            effects[i].onTurnEnd(this);
        OnEffectsChanged?.Invoke();
    }


    public UnitAIType getAIType()
    {
        if (unitData == null)
            return UnitAIType.None;

        return unitData.aiType;
    }
}