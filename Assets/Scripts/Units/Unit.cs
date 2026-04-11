using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
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

    private UnitStatsUI statsUI;

    public event Action OnEffectsChanged;


    public void init(UnitData data, UnitType type)
    {
        unitData = data;
        unitType = type;
        unitName = data.unitName;
        maxHealth = data.maxHealth;
        currentMaxHealth = maxHealth;
        currentHealth = maxHealth;

        foreach (var effect in data.startEffects)
            if (effect != null) addEffect(effect.Clone());

        if (UnitStatsUIManager.Instance != null)
            UnitStatsUIManager.Instance.createStatsUI(this);

        if (UnitsManager.Instance != null)
        {
            if (type == UnitType.Player)
                UnitsManager.Instance.player = this;
            else if (type == UnitType.Ally)
                UnitsManager.Instance.addAlly(this);
            else if (type == UnitType.Enemy)
                UnitsManager.Instance.addEnemy(this);
        }
    }


    void Start()
    {
        currentMaxHealth = maxHealth;
        currentHealth = maxHealth;

        if (UnitStatsUIManager.Instance != null)
        {
            UnitStatsUIManager.Instance.createStatsUI(this);
        }
        ;

        if (unitData != null)
            init(unitData, unitType);
        else if (unitType == UnitType.Player)
        {
            if (UnitsManager.Instance != null)
                UnitsManager.Instance.player = this;
        }
    }


    public void setStatsUI(UnitStatsUI ui)
    {
        statsUI = ui;
    }


    public void takeTurn()
    {
        if (unitType == UnitType.Player) return;

        onEffectsTurnStart();

        if (unitData == null || unitData.moves == null || unitData.moves.Count == 0) return;

        int randomIndex = Random.Range(0, unitData.moves.Count);
        UnitMove selectedMove = unitData.moves[randomIndex];

        Debug.Log($"{unitName} uses {selectedMove.moveName}");

        foreach (var action in selectedMove.actions)
        {
            if (action == null) continue;

            action.execute(UnitsManager.Instance.player, this);
        }

        onEffectsTurnEnd();
    }


    public void takeDamage(int damage, DamageType type = DamageType.Normal, Unit source = null)
    {
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

        if (currentHealth <= 0)
            Destroy(gameObject);
    }


    public void addBlock(int amount)
    {
        block += amount;
        statsUI?.updateUI();
    }


    public void heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > currentMaxHealth)
            currentHealth = currentMaxHealth;

        statsUI?.updateUI();
    }


    public void resetBlock()
    {
        block = 0;
        statsUI?.updateUI();
    }


    public void addEffect(BaseStatusEffect newEffect)
    {
        Debug.Log($"addEffect: {newEffect.effectName} (stacks: {newEffect.getMainText()}) do {unitName}");

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
        foreach (var effect in effects.ToList())
            effect.onTurnStart(this);

        OnEffectsChanged?.Invoke();
    }


    public void onEffectsTurnEnd()
    {
        foreach (var effect in effects.ToList())
            effect.onTurnEnd(this);

        OnEffectsChanged?.Invoke();
    }
}