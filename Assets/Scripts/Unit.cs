using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int health;

    public void takeDamage(int dmg)
    {
        Debug.Log($"{unitName} takes {dmg} damage");
    }

    public void heal(int amount)
    {
        Debug.Log($"{unitName} heals {amount}");
    }

    public void addBlock(int amount)
    {
        Debug.Log($"{unitName} gains {amount} block");
    }
}