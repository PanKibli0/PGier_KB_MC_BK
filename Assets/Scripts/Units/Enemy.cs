using UnityEngine;

public class Enemy : Unit
{
    [SerializeField] private EnemyData data;


    new void Start()
    {
        if (data == null) return;

        unitName = data.enemyName;
        maxHealth = data.maxHealth;
        currentHealth = maxHealth;
        unitType = UnitType.Enemy;

        base.Start();
    }


    public void takeTurn()
    {
        
        if (data == null || data.moves == null || data.moves.Count == 0) return;

        int randomIndex = Random.Range(0, data.moves.Count);
        EnemyMove selectedMove = data.moves[randomIndex];

        Debug.Log($"{unitName} uses {selectedMove.moveName}");

        foreach (var action in selectedMove.actions)
        {
            if (action == null) continue;

            // DEBUG: cel - gracz (singleton)
            action.execute(Player.Instance);
            // END DEBUG
        }
    }
}