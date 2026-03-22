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
        // TODO: rozbudować o block w przyszłości
        currentHealth -= damage;
        Debug.Log($"{unitName} otrzymuje {damage} obrażeń. Health: {currentHealth}/{maxHealth}");


        statsUI?.updateUI();

        if (currentHealth <= 0)
        {
            Debug.Log($"{unitName} został pokonany!");
            Destroy(gameObject);
        }
    }

}


