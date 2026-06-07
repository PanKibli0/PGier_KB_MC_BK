using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{
    private const int COLUMNS = 5;
    private const int FLOORS = 13;
    private const int PATH_COUNT = 6;

    private const int FLOOR_BATTLE = 1;
    private const int FLOOR_RELIC = 5;
    private const int FLOOR_REST = 12;
    private const int FLOOR_BOSS = 13;

    private float[] zone1Weights = { 60f, 22f, 6f, 0f, 2f, 10f };
    private float[] zone2Weights = { 42f, 20f, 8f, 10f, 10f, 10f };
    private float[] zone3Weights = { 35f, 16f, 8f, 14f, 15f, 12f };

    public enum NodeType { Battle, Event, Shop, Rest, Elite, Relic, Boss }

    private bool[,] grid;
    private List<int>[,] connections;
    private NodeType[,] types;
    private EnemyPool pool;

    public List<BaseNode> generateMap(EnemyPool enemyPool)
    {
        pool = enemyPool;
        grid = new bool[COLUMNS, FLOORS];
        connections = new List<int>[COLUMNS, FLOORS];
        types = new NodeType[COLUMNS, FLOORS];

        for (int c = 0; c < COLUMNS; c++)
            for (int f = 0; f < FLOORS; f++)
                connections[c, f] = new List<int>();

        generatePaths();
        assignTypes();
        return buildNodes();
    }

    private void generatePaths()
    {
        HashSet<int> usedStarts = new HashSet<int>();

        for (int p = 0; p < PATH_COUNT; p++)
        {
            int startCol;
            int attempts = 0;
            do
            {
                startCol = Random.Range(0, COLUMNS);
                attempts++;
            }
            while (usedStarts.Count < 2 && usedStarts.Contains(startCol) && attempts < 20);

            usedStarts.Add(startCol);

            int currentCol = startCol;
            grid[currentCol, 0] = true;

            for (int f = 0; f < FLOORS - 2; f++)
            {
                int nextCol = pickNextColumn(currentCol, f);
                grid[nextCol, f + 1] = true;

                if (!wouldCross(currentCol, nextCol, f))
                    connections[currentCol, f].Add(nextCol);
                else
                {
                    grid[currentCol, f + 1] = true;
                    connections[currentCol, f].Add(currentCol);
                    nextCol = currentCol;
                }

                currentCol = nextCol;
            }
        }

        for (int c = 0; c < COLUMNS; c++)
        {
            for (int f = 0; f < FLOORS; f++)
            {
                List<int> unique = new List<int>();
                foreach (int x in connections[c, f])
                    if (!unique.Contains(x))
                        unique.Add(x);
                connections[c, f] = unique;
            }
        }
    }

    private int pickNextColumn(int current, int floor)
    {
        List<int> candidates = new List<int>();
        for (int dc = -1; dc <= 1; dc++)
        {
            int nc = current + dc;
            if (nc >= 0 && nc < COLUMNS)
                candidates.Add(nc);
        }

        List<int> safe = new List<int>();
        foreach (int nc in candidates)
            if (!wouldCross(current, nc, floor))
                safe.Add(nc);

        if (safe.Count > 0)
            return safe[Random.Range(0, safe.Count)];

        return candidates[Random.Range(0, candidates.Count)];
    }

    private bool wouldCross(int fromCol, int toCol, int floor)
    {
        for (int c = 0; c < COLUMNS; c++)
        {
            foreach (int nc in connections[c, floor])
            {
                if (c < fromCol && nc > toCol) return true;
                if (c > fromCol && nc < toCol) return true;
            }
        }
        return false;
    }

    private void assignTypes()
    {
        for (int c = 0; c < COLUMNS; c++)
        {
            for (int f = 0; f < FLOORS; f++)
            {
                if (!grid[c, f]) continue;

                int floor1 = f + 1;

                if (floor1 == FLOOR_BATTLE) { types[c, f] = NodeType.Battle; continue; }
                if (floor1 == FLOOR_RELIC) { types[c, f] = NodeType.Relic; continue; }
                if (floor1 == FLOOR_REST) { types[c, f] = NodeType.Rest; continue; }
                if (floor1 == FLOOR_BOSS) { types[c, f] = NodeType.Boss; continue; }

                types[c, f] = rollType(getZoneWeights(floor1));
            }
        }

        applyExclusionRules();
    }

    private float[] getZoneWeights(int floor1)
    {
        if (floor1 >= 2 && floor1 <= 4) return zone1Weights;
        if (floor1 >= 6 && floor1 <= 8) return zone2Weights;
        if (floor1 >= 9 && floor1 <= 11) return zone3Weights;
        return zone2Weights;
    }

    private NodeType rollType(float[] weights)
    {
        float total = 0f;
        foreach (float w in weights) total += w;

        float roll = Random.Range(0f, total);
        float cumulative = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (roll <= cumulative)
                return (NodeType)i;
        }

        return NodeType.Battle;
    }

    private void applyExclusionRules()
    {
        HashSet<NodeType> noConsecutive = new HashSet<NodeType>
    {
        NodeType.Rest, NodeType.Elite, NodeType.Shop
    };

        for (int pass = 0; pass < 3; pass++)
        {
            for (int c = 0; c < COLUMNS; c++)
            {
                for (int f = 0; f < FLOORS - 1; f++)
                {
                    if (!grid[c, f]) continue;
                    NodeType currentType = types[c, f];

                    for (int nc = 0; nc < COLUMNS; nc++)
                    {
                        if (!grid[nc, f + 1]) continue;
                        if (Mathf.Abs(nc - c) > 1) continue;
                        if (types[nc, f + 1] != currentType) continue;

                        bool shouldReroll = noConsecutive.Contains(currentType);

                        if (!shouldReroll && currentType == NodeType.Event && f >= 1)
                        {
                            for (int pc = 0; pc < COLUMNS; pc++)
                            {
                                if (!grid[pc, f - 1]) continue;
                                if (Mathf.Abs(pc - c) > 1) continue;
                                if (types[pc, f - 1] == NodeType.Event)
                                {
                                    shouldReroll = true;
                                    break;
                                }
                            }
                        }

                        if (!shouldReroll) continue;

                        float[] weights = getZoneWeights(f + 2);
                        NodeType newType;
                        int tries = 0;
                        do
                        {
                            newType = rollType(weights);
                            tries++;
                        }
                        while (newType == currentType && tries < 10);

                        types[nc, f + 1] = newType;
                    }
                }
            }
        }
    }

    private List<BaseNode> buildNodes()
    {
        BaseNode[,] nodeMap = new BaseNode[COLUMNS, FLOORS];
        List<BaseNode> allNodes = new List<BaseNode>();

        for (int f = 0; f < FLOORS - 1; f++)
        {
            for (int c = 0; c < COLUMNS; c++)
            {
                if (!grid[c, f]) continue;

                BaseNode node = createNode(types[c, f]);
                node.gridPosition = new Vector2Int(c, f);
                node.isUnlocked = (f == 0);
                node.visitedIconPath = $"Icons_map/X_{Random.Range(1, 4)}";

                nodeMap[c, f] = node;
                allNodes.Add(node);
            }
        }

        BattleNode bossNode = createBossNode();
        bossNode.gridPosition = new Vector2Int(COLUMNS / 2, FLOORS - 1);
        bossNode.isUnlocked = false;
        bossNode.visitedIconPath = $"Icons_map/X_{Random.Range(1, 4)}";
        allNodes.Add(bossNode);

        for (int f = 0; f < FLOORS - 2; f++)
        {
            for (int c = 0; c < COLUMNS; c++)
            {
                if (nodeMap[c, f] == null) continue;

                foreach (int nc in connections[c, f])
                {
                    if (nodeMap[nc, f + 1] != null)
                        nodeMap[c, f].connections.Add(nodeMap[nc, f + 1]);
                }

                if (nodeMap[c, f].connections.Count == 0)
                {
                    for (int dc = 0; dc <= COLUMNS; dc++)
                    {
                        bool found = false;

                        if (c - dc >= 0 && nodeMap[c - dc, f + 1] != null)
                        {
                            nodeMap[c, f].connections.Add(nodeMap[c - dc, f + 1]);
                            found = true;
                        }
                        else if (c + dc < COLUMNS && nodeMap[c + dc, f + 1] != null)
                        {
                            nodeMap[c, f].connections.Add(nodeMap[c + dc, f + 1]);
                            found = true;
                        }

                        if (found) break;
                    }
                }
            }
        }

        int restIdx = FLOOR_REST - 1;

        for (int c = 0; c < COLUMNS; c++)
        {
            if (nodeMap[c, restIdx - 1] == null) continue;
            if (nodeMap[c, restIdx] != null)
            {
                nodeMap[c, restIdx - 1].connections.Add(nodeMap[c, restIdx]);
                continue;
            }

            for (int dc = 1; dc < COLUMNS; dc++)
            {
                bool found = false;

                if (c - dc >= 0 && nodeMap[c - dc, restIdx] != null)
                {
                    nodeMap[c, restIdx - 1].connections.Add(nodeMap[c - dc, restIdx]);
                    found = true;
                }
                else if (c + dc < COLUMNS && nodeMap[c + dc, restIdx] != null)
                {
                    nodeMap[c, restIdx - 1].connections.Add(nodeMap[c + dc, restIdx]);
                    found = true;
                }

                if (found) break;
            }
        }

        for (int c = 0; c < COLUMNS; c++)
        {
            if (nodeMap[c, restIdx] != null)
                nodeMap[c, restIdx].connections.Add(bossNode);
        }

        return allNodes;
    }

    private BaseNode createNode(NodeType type)
    {
        switch (type)
        {
            case NodeType.Battle:
                {
                    BattleNode battleNode = new BattleNode();
                    battleNode.difficulty = BattleDifficulty.Normal;
                    battleNode.enemies = pool.normalFights[Random.Range(0, pool.normalFights.Count)].enemies;
                    return battleNode;
                }
            case NodeType.Elite:
                {
                    BattleNode eliteNode = new BattleNode();
                    eliteNode.difficulty = BattleDifficulty.Elite;
                    eliteNode.enemies = pool.eliteFights[Random.Range(0, pool.eliteFights.Count)].enemies;
                    return eliteNode;
                }
            case NodeType.Relic:
                return new RelicNode();

            case NodeType.Shop:
                return new ShopNode();

            case NodeType.Rest:
                return new RestNode();

            case NodeType.Event:
                return new EventNode();

            default:
                {
                    BattleNode battleNode = new BattleNode();
                    battleNode.difficulty = BattleDifficulty.Normal;
                    battleNode.enemies = pool.normalFights[Random.Range(0, pool.normalFights.Count)].enemies;
                    return battleNode;
                }
        }
    }

    private BattleNode createBossNode()
    {
        BattleNode bossNode = new BattleNode();
        bossNode.difficulty = BattleDifficulty.Boss;
        if (pool.bossFights != null && pool.bossFights.Count > 0)
            bossNode.enemies = pool.bossFights[Random.Range(0, pool.bossFights.Count)].enemies;
        return bossNode;
    }
}