using UnityEngine;

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

    private UnitStatsUI statsUI;

    protected void Start()
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

}


