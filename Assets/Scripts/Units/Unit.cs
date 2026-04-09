using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum UnitType
{
    Player,
    Ally,
    Enemy
}

public class Unit : MonoBehaviour
{
    public string unitName;
    public UnitType unitType;

    public int maxHealth;
    public int currentmaxHealth;
    public int currentHealth;
    public int block;

    [SerializeReference] public List<BaseStatusEffect> effects = new List<BaseStatusEffect>();

    private UnitStatsUI statsUI;


    protected virtual void Start()
    {

        currentmaxHealth = maxHealth;
        currentHealth = maxHealth;


        if (UnitStatsUIManager.Instance != null)
        {
            UnitStatsUIManager.Instance.createStatsUI(this);
        }
    }


    public void setStatsUI(UnitStatsUI ui)
    {
        statsUI = ui;
    }


    public void takeDamage(int damage)
    {
        if (block > 0)
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

    public void takeTrueDamage(int damage)
    {
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
        if (currentHealth > currentmaxHealth)
           currentHealth = currentmaxHealth;
        
        statsUI?.updateUI();
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

                    statsUI?.updateEffectsUI();
                    return;
                }
            }
        }

        effects.Add(newEffect);
        newEffect.onApply(this);
        statsUI?.updateEffectsUI();
    }

    public void removeEffect(BaseStatusEffect effect)
    {
        effects.Remove(effect);
        statsUI?.updateEffectsUI();
    }

    public void onEffectsTurnStart()
    {
        foreach (var effect in effects.ToList())
            effect.onTurnStart(this);

        statsUI?.updateEffectsUI();
    }

    public void onEffectsTurnEnd()
    {
        foreach (var effect in effects.ToList())
            effect.onTurnEnd(this);

        statsUI?.updateEffectsUI();
    }
}


