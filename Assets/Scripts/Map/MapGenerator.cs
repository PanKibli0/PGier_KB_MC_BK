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

        for (int col = 0; col < COLUMNS; col++)
            for (int floor = 0; floor < FLOORS; floor++)
                connections[col, floor] = new List<int>();

        generatePaths();
        assignTypes();
        return buildNodes();
    }

    private void generatePaths()
    {
        HashSet<int> usedStartColumns = new HashSet<int>();

        for (int pathIndex = 0; pathIndex < PATH_COUNT; pathIndex++)
        {
            int startCol;
            int attempts = 0;
            do
            {
                startCol = Random.Range(0, COLUMNS);
                attempts++;
            }
            while (usedStartColumns.Count < 2 && usedStartColumns.Contains(startCol) && attempts < 20);

            usedStartColumns.Add(startCol);

            int currentCol = startCol;
            grid[currentCol, 0] = true;

            for (int floor = 0; floor < FLOORS - 2; floor++)
            {
                int nextCol = pickNextColumn(currentCol, floor);
                grid[nextCol, floor + 1] = true;

                if (!wouldCross(currentCol, nextCol, floor))
                    connections[currentCol, floor].Add(nextCol);
                else
                {
                    grid[currentCol, floor + 1] = true;
                    connections[currentCol, floor].Add(currentCol);
                    nextCol = currentCol;
                }

                currentCol = nextCol;
            }
        }

        for (int col = 0; col < COLUMNS; col++)
        {
            for (int floor = 0; floor < FLOORS; floor++)
            {
                List<int> uniqueConnections = new List<int>();
                foreach (int connectedCol in connections[col, floor])
                    if (!uniqueConnections.Contains(connectedCol))
                        uniqueConnections.Add(connectedCol);
                connections[col, floor] = uniqueConnections;
            }
        }
    }

    private int pickNextColumn(int currentCol, int floor)
    {
        List<int> candidateCols = new List<int>();
        for (int offset = -1; offset <= 1; offset++)
        {
            int neighborCol = currentCol + offset;
            if (neighborCol >= 0 && neighborCol < COLUMNS)
                candidateCols.Add(neighborCol);
        }

        List<int> safeCols = new List<int>();
        foreach (int neighborCol in candidateCols)
            if (!wouldCross(currentCol, neighborCol, floor))
                safeCols.Add(neighborCol);

        if (safeCols.Count > 0)
            return safeCols[Random.Range(0, safeCols.Count)];

        return candidateCols[Random.Range(0, candidateCols.Count)];
    }

    private bool wouldCross(int fromCol, int toCol, int floor)
    {
        for (int col = 0; col < COLUMNS; col++)
        {
            foreach (int existingTarget in connections[col, floor])
            {
                if (col < fromCol && existingTarget > toCol) return true;
                if (col > fromCol && existingTarget < toCol) return true;
            }
        }
        return false;
    }

    private void assignTypes()
    {
        for (int col = 0; col < COLUMNS; col++)
        {
            for (int floor = 0; floor < FLOORS; floor++)
            {
                if (!grid[col, floor]) continue;

                int floorNumber = floor + 1;

                if (floorNumber == FLOOR_BATTLE) { types[col, floor] = NodeType.Battle; continue; }
                if (floorNumber == FLOOR_RELIC) { types[col, floor] = NodeType.Relic; continue; }
                if (floorNumber == FLOOR_REST) { types[col, floor] = NodeType.Rest; continue; }
                if (floorNumber == FLOOR_BOSS) { types[col, floor] = NodeType.Boss; continue; }

                types[col, floor] = rollType(getZoneWeights(floorNumber));
            }
        }

        applyExclusionRules();
    }

    private float[] getZoneWeights(int floorNumber)
    {
        if (floorNumber >= 2 && floorNumber <= 4) return zone1Weights;
        if (floorNumber >= 6 && floorNumber <= 8) return zone2Weights;
        if (floorNumber >= 9 && floorNumber <= 11) return zone3Weights;
        return zone2Weights;
    }

    private NodeType rollType(float[] weights)
    {
        float totalWeight = 0f;
        foreach (float weight in weights) totalWeight += weight;

        float roll = Random.Range(0f, totalWeight);
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
        HashSet<NodeType> noConsecutiveTypes = new HashSet<NodeType>
        {
            NodeType.Rest, NodeType.Elite, NodeType.Shop
        };

        for (int pass = 0; pass < 3; pass++)
        {
            for (int col = 0; col < COLUMNS; col++)
            {
                for (int floor = 0; floor < FLOORS - 1; floor++)
                {
                    if (!grid[col, floor]) continue;
                    NodeType currentType = types[col, floor];

                    for (int neighborCol = 0; neighborCol < COLUMNS; neighborCol++)
                    {
                        if (!grid[neighborCol, floor + 1]) continue;
                        if (Mathf.Abs(neighborCol - col) > 1) continue;
                        if (types[neighborCol, floor + 1] != currentType) continue;

                        bool shouldReroll = noConsecutiveTypes.Contains(currentType);

                        if (!shouldReroll && currentType == NodeType.Event && floor >= 1)
                        {
                            for (int prevCol = 0; prevCol < COLUMNS; prevCol++)
                            {
                                if (!grid[prevCol, floor - 1]) continue;
                                if (Mathf.Abs(prevCol - col) > 1) continue;
                                if (types[prevCol, floor - 1] == NodeType.Event)
                                {
                                    shouldReroll = true;
                                    break;
                                }
                            }
                        }

                        if (!shouldReroll) continue;

                        float[] weights = getZoneWeights(floor + 2);
                        NodeType newType;
                        int tries = 0;
                        do
                        {
                            newType = rollType(weights);
                            tries++;
                        }
                        while (newType == currentType && tries < 10);

                        types[neighborCol, floor + 1] = newType;
                    }
                }
            }
        }
    }

    private List<BaseNode> buildNodes()
    {
        BaseNode[,] nodeMap = new BaseNode[COLUMNS, FLOORS];
        List<BaseNode> allNodes = new List<BaseNode>();

        for (int floor = 0; floor < FLOORS - 1; floor++)
        {
            for (int col = 0; col < COLUMNS; col++)
            {
                if (!grid[col, floor]) continue;

                BaseNode node = createNode(types[col, floor]);
                node.gridPosition = new Vector2Int(col, floor);
                node.isUnlocked = (floor == 0);
                node.visitedIconPath = $"Icons_map/X_{Random.Range(1, 4)}";

                nodeMap[col, floor] = node;
                allNodes.Add(node);
            }
        }

        BattleNode bossNode = createBossNode();
        bossNode.gridPosition = new Vector2Int(COLUMNS / 2, FLOORS - 1);
        bossNode.isUnlocked = false;
        bossNode.visitedIconPath = $"Icons_map/X_{Random.Range(1, 4)}";
        allNodes.Add(bossNode);

        for (int floor = 0; floor < FLOORS - 2; floor++)
        {
            for (int col = 0; col < COLUMNS; col++)
            {
                if (nodeMap[col, floor] == null) continue;

                foreach (int targetCol in connections[col, floor])
                {
                    if (nodeMap[targetCol, floor + 1] != null)
                        nodeMap[col, floor].connections.Add(nodeMap[targetCol, floor + 1]);
                }

                if (nodeMap[col, floor].connections.Count == 0)
                {
                    for (int distance = 0; distance <= COLUMNS; distance++)
                    {
                        bool found = false;

                        if (col - distance >= 0 && nodeMap[col - distance, floor + 1] != null)
                        {
                            nodeMap[col, floor].connections.Add(nodeMap[col - distance, floor + 1]);
                            found = true;
                        }
                        else if (col + distance < COLUMNS && nodeMap[col + distance, floor + 1] != null)
                        {
                            nodeMap[col, floor].connections.Add(nodeMap[col + distance, floor + 1]);
                            found = true;
                        }

                        if (found) break;
                    }
                }
            }
        }

        int restFloorIndex = FLOOR_REST - 1;

        for (int col = 0; col < COLUMNS; col++)
        {
            if (nodeMap[col, restFloorIndex - 1] == null) continue;
            if (nodeMap[col, restFloorIndex] != null)
            {
                nodeMap[col, restFloorIndex - 1].connections.Add(nodeMap[col, restFloorIndex]);
                continue;
            }

            for (int distance = 1; distance < COLUMNS; distance++)
            {
                bool found = false;

                if (col - distance >= 0 && nodeMap[col - distance, restFloorIndex] != null)
                {
                    nodeMap[col, restFloorIndex - 1].connections.Add(nodeMap[col - distance, restFloorIndex]);
                    found = true;
                }
                else if (col + distance < COLUMNS && nodeMap[col + distance, restFloorIndex] != null)
                {
                    nodeMap[col, restFloorIndex - 1].connections.Add(nodeMap[col + distance, restFloorIndex]);
                    found = true;
                }

                if (found) break;
            }
        }

        for (int col = 0; col < COLUMNS; col++)
        {
            if (nodeMap[col, restFloorIndex] != null)
                nodeMap[col, restFloorIndex].connections.Add(bossNode);
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