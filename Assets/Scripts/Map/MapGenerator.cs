using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{
    public static List<BaseNode> generateMap(EnemyPool pool)
    {
        List<BaseNode> nodes = new List<BaseNode>();

        
        BattleNode node11 = new BattleNode();
        node11.enemies = pool.normalFights[Random.Range(0, pool.normalFights.Count)].enemies;
        node11.difficulty = BattleDifficulty.Normal;
        node11.isUnlocked = true;
        node11.gridPosition = new Vector2Int(3, 0);
        


        BattleNode node21 = new BattleNode();
        node21.enemies = pool.normalFights[Random.Range(0, pool.normalFights.Count)].enemies;
        node21.difficulty = BattleDifficulty.Normal;
        node21.gridPosition = new Vector2Int(2, 1);

        BattleNode node22 = new BattleNode();
        node22.enemies = pool.normalFights[Random.Range(0, pool.normalFights.Count)].enemies;
        node22.difficulty = BattleDifficulty.Normal;
        node22.gridPosition = new Vector2Int(4, 1);

        BattleNode node31 = new BattleNode();
        node31.enemies = pool.normalFights[Random.Range(0, pool.normalFights.Count)].enemies;
        node31.difficulty = BattleDifficulty.Normal;
        node31.gridPosition = new Vector2Int(2, 2);

        BattleNode node32 = new BattleNode();
        node32.enemies = pool.eliteFights[0].enemies;
        node32.difficulty = BattleDifficulty.Elite;
        node32.gridPosition = new Vector2Int(3, 2);

        BattleNode node33 = new BattleNode();
        node33.enemies = pool.normalFights[Random.Range(0, pool.normalFights.Count)].enemies;
        node33.difficulty = BattleDifficulty.Normal;
        node33.gridPosition = new Vector2Int(4, 2);

        //node33.gridPosition = new Vector2Int(4, 10);

        node11.connections = new List<BaseNode> { node21, node22 };
        node21.connections = new List<BaseNode> { node31 };
        node22.connections = new List<BaseNode> { node32, node33 };

        nodes.Add(node11);
        nodes.Add(node21);
        nodes.Add(node22);
        nodes.Add(node31);
        nodes.Add(node32);
        nodes.Add(node33);

        return nodes;
    }
}