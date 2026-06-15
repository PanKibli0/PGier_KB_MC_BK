using System;
using System.Collections.Generic;
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
    public UnitData unitData;

    public int maxHealth;
    public int currentMaxHealth;
    public int currentHealth;
    public int block;

    public Animator animator;

    [SerializeReference] public List<BaseStatusEffect> effects = new List<BaseStatusEffect>();
    public UnitMove nextMove;

    private UnitStatsUI statsUI;

    public event Action OnEffectsChanged;

    private bool isDead = false;

    private RelicManager relics;
    private UnitsManager unitsManager;

    public void PlayAnimation(string triggerName)
    {
        if (animator == null) return;
        animator.SetTrigger(triggerName);
    }


    public void init(BaseUnitData data, UnitType type, UnitStatsUIManager statsUIManager, 
        UnitsManager unitsManager)
    {
        this.unitsManager = unitsManager;
        unitName = data.unitName;
        unitType = type;

        if (data.minHealth == 0) maxHealth = data.maxHealth;
        else maxHealth = Random.Range(data.minHealth, data.maxHealth);

        currentMaxHealth = maxHealth;
        currentHealth = maxHealth;

        foreach (var effect in data.startEffects)
            if (effect != null)
                addEffect(effect.Clone());

        if (data is UnitData uData)
            unitData = uData;

        statsUIManager.createStatsUI(this);
        relics = GameManager.Instance.relicManager;

        animator = GetComponentInChildren<Animator>();
        if (animator != null)
            animator.Play("Idle"); // domyœlnie idle
    }

    public void setStatsUI(UnitStatsUI ui)
    {
        statsUI = ui;
    }

    public void showIntent(UnitMove move)
    {
        nextMove = move;
        statsUI?.showIntent(nextMove);
    }

    public void hideIntent()
    {
        nextMove = null;
        statsUI?.hideIntent();
    }

    public void takeDamage(int damage, DamageType type = DamageType.Normal, Unit source = null)
    {
        if (isDead) return;

        if (type != DamageType.Pure)
        {
            for (int i = effects.Count - 1; i >= 0; i--)
                effects[i].onReceiveDamage(this, source, ref damage);
        }

        if (unitType == UnitType.Player)
            relics.onDamageTaken(this, source);

        if (type == DamageType.Normal && block > 0)
        {
            int blockUsed = Mathf.Min(block, damage);
            block -= blockUsed;
            damage -= blockUsed;
        }

        if (damage > 0)
            for (int i = effects.Count - 1; i >= 0; i--)
                effects[i].onHealthLost(this, damage);

        currentHealth -= damage;

        statsUI?.updateUI();

        PlayAnimation("Hurt");

        if (unitType == UnitType.Player && GameManager.Instance != null) 
            GameManager.Instance.setHealth(currentHealth);

        if (currentHealth <= 0 && !isDead)
            die();
    }

    

    public void die()
    {
        PlayAnimation("Death");
        isDead = true;
        if (unitType == UnitType.Enemy)
            GameManager.Instance?.addEnemyKill();
        unitsManager.onUnitDied(this);
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
            GameManager.Instance.setHealth(currentHealth);
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