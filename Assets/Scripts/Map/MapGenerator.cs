using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{
    public static List<BaseNode> generateMap(EnemyPool pool)
    {
        List<BaseNode> nodes = new List<BaseNode>();

        // Węzeł 1.1 (row 0, col 3) - dół, środek
        BattleNode node11 = new BattleNode();
        node11.enemies = pool.normalFights[0].enemies;
        node11.difficulty = BattleDifficulty.Normal;
        node11.isUnlocked = true;
        node11.isVisited = false;
        node11.gridPosition = new Vector2Int(0, 3);

        // Węzeł 2.1 (row 1, col 2) - lewy
        BattleNode node21 = new BattleNode();
        node21.enemies = pool.normalFights[0].enemies;
        node21.difficulty = BattleDifficulty.Normal;
        node21.isUnlocked = false;
        node21.isVisited = false;
        node21.gridPosition = new Vector2Int(1, 2);

        // Węzeł 2.2 (row 1, col 4) - prawy
        BattleNode node22 = new BattleNode();
        node22.enemies = pool.normalFights[0].enemies;
        node22.difficulty = BattleDifficulty.Normal;
        node22.isUnlocked = false;
        node22.isVisited = false;
        node22.gridPosition = new Vector2Int(1, 4);

        // Węzeł 3.1 (row 2, col 1)
        BattleNode node31 = new BattleNode();
        node31.enemies = pool.normalFights[1].enemies;
        node31.difficulty = BattleDifficulty.Normal;
        node31.isUnlocked = false;
        node31.isVisited = false;
        node31.gridPosition = new Vector2Int(2, 1);

        // Węzeł 3.2 (row 2, col 5)
        BattleNode node32 = new BattleNode();
        node32.enemies = pool.eliteFights[0].enemies;
        node32.difficulty = BattleDifficulty.Elite;
        node32.isUnlocked = false;
        node32.isVisited = false;
        node32.gridPosition = new Vector2Int(2, 5);

        // Połączenia
        node11.connections = new List<BaseNode> { node21, node22 };
        node21.connections = new List<BaseNode> { node31 };
        node22.connections = new List<BaseNode> { node32 };

        nodes.Add(node11);
        nodes.Add(node21);
        nodes.Add(node22);
        nodes.Add(node31);
        nodes.Add(node32);

        return nodes;
    }
}