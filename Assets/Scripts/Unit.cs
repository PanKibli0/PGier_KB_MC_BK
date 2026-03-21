using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int health;
    public UnitType unitType; 

    public void takeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{unitName} took {damage} damage. Remaining health: {health}");
        if (health <= 0)
        {
            Debug.Log($"{unitName} has been defeated!");
            Destroy(gameObject);
        }
    }
}

public enum UnitType
{
    Player,
    Ally,
    Enemy
}