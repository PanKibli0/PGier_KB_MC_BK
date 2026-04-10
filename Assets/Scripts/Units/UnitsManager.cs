using UnityEngine;
using System.Collections.Generic;

public class UnitsManager : MonoBehaviour
{
    public static UnitsManager Instance;

    public Unit player;
    public List<Unit> allies = new List<Unit>();
    public List<Unit> enemies = new List<Unit>();


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void addAlly(Unit ally)
    {
        if (allies.Count >= 3) return;
        allies.Add(ally);
    }

    public void addEnemy(Unit enemy)
    {
        if (enemies.Count >= 4) return;
        enemies.Add(enemy);
    }
}