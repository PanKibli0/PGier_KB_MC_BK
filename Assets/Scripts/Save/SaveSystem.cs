using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string characterName;
    public int currentHealth;
    public int maxHealth;
    public int gold;
    public int enemiesKilled;
    public int floorsCompleted;
    public List<string> deckCardNames = new List<string>();
    public List<string> relicNames = new List<string>();
    public List<string> itemNames = new List<string>();
    public SavedMapData map = new SavedMapData();
}

[Serializable]
public class SavedMapData
{
    public int currentFloor;
    public List<SavedNode> nodes = new List<SavedNode>();
}

[Serializable]
public class SavedNode
{
    public int col;
    public int floor;
    public string nodeType;
    public string difficulty;
    public bool isVisited;
    public bool isUnlocked;
    public List<string> enemyNames = new List<string>();
    public string eventName;
    public List<int> connectionCols = new List<int>();
    public List<int> connectionFloors = new List<int>();
}

public static class SaveSystem
{
    private static string savePath()
    {
        return Path.Combine(Application.persistentDataPath, "save.json");
    }

    public static bool saveExists()
    {
        return File.Exists(savePath());
    }

    public static void deleteSave()
    {
        if (saveExists())
            File.Delete(savePath());
    }

    public static void save(GameManager gm)
    {
        if (gm.currentMap == null || gm.selectedCharacter == null) return;

        SaveData data = new SaveData();

        data.characterName = gm.selectedCharacter.name;
        data.currentHealth = gm.currentHealth;
        data.maxHealth = gm.maxHealth;
        data.gold = gm.gold;
        data.enemiesKilled = gm.enemiesKilled;
        data.floorsCompleted = gm.floorsCompleted;

        foreach (CardData card in gm.currentDeck)
            data.deckCardNames.Add(card.name);

        foreach (RelicData relic in gm.relicManager.getRelics())
            data.relicNames.Add(relic.name);

        foreach (ItemData item in gm.playerInventory.items)
            data.itemNames.Add(item.name);

        data.map = saveMap(gm.currentMap);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath(), json);

        Debug.Log("Zapisano: " + savePath());
    }

    private static SavedMapData saveMap(MapData map)
    {
        SavedMapData savedMap = new SavedMapData();
        savedMap.currentFloor = map.currentFloor;

        foreach (BaseNode node in map.nodes)
        {
            SavedNode savedNode = new SavedNode();
            savedNode.col = node.gridPosition.x;
            savedNode.floor = node.gridPosition.y;
            savedNode.isVisited = node.isVisited;
            savedNode.isUnlocked = node.isUnlocked;

            if (node is BattleNode battleNode)
            {
                savedNode.difficulty = battleNode.difficulty.ToString();

                if (battleNode.difficulty == BattleDifficulty.Boss)
                    savedNode.nodeType = "Boss";
                else if (battleNode.difficulty == BattleDifficulty.Elite)
                    savedNode.nodeType = "Elite";
                else
                    savedNode.nodeType = "Battle";

                if (battleNode.enemies != null)
                    foreach (UnitData enemy in battleNode.enemies)
                        savedNode.enemyNames.Add(enemy.name);
            }
            else if (node is ShopNode)
                savedNode.nodeType = "Shop";
            else if (node is RestNode)
                savedNode.nodeType = "Rest";
            else if (node is RelicNode)
                savedNode.nodeType = "Relic";
            else if (node is EventNode eventNode)
            {
                savedNode.nodeType = "Event";
                if (eventNode.eventData != null)
                    savedNode.eventName = eventNode.eventData.name;
            }

            foreach (BaseNode connection in node.connections)
            {
                savedNode.connectionCols.Add(connection.gridPosition.x);
                savedNode.connectionFloors.Add(connection.gridPosition.y);
            }

            savedMap.nodes.Add(savedNode);
        }

        return savedMap;
    }

    public static bool load(GameManager gm)
    {
        if (!saveExists()) return false;

        string json = File.ReadAllText(savePath());
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (data == null) return false;

        SaveDatabase db = gm.saveDatabase;

        gm.selectedCharacter = db.findCharacter(data.characterName);
        gm.currentHealth = data.currentHealth;
        gm.maxHealth = data.maxHealth;
        gm.gold = data.gold;
        gm.enemiesKilled = data.enemiesKilled;
        gm.floorsCompleted = data.floorsCompleted;

        gm.currentDeck = new List<CardData>();
        foreach (string cardName in data.deckCardNames)
        {
            CardData card = db.findCard(cardName);
            if (card != null)
                gm.currentDeck.Add(card);
        }

        List<RelicData> relics = new List<RelicData>();
        foreach (string relicName in data.relicNames)
        {
            RelicData relic = db.findRelic(relicName);
            if (relic != null)
                relics.Add(relic);
        }
        gm.relicManager = new RelicManager(relics);

        gm.playerInventory = new PlayerInventory();
        foreach (string itemName in data.itemNames)
        {
            ItemData item = db.findItem(itemName);
            if (item != null)
                gm.playerInventory.addItem(item);
        }

        gm.currentMap = loadMap(data.map, db);

        Debug.Log("Wczytano: " + savePath());
        return true;
    }

    private static MapData loadMap(SavedMapData savedMap, SaveDatabase db)
    {
        MapData map = new MapData();
        map.currentFloor = savedMap.currentFloor;
        map.nodes = new List<BaseNode>();

        BaseNode[,] nodeGrid = new BaseNode[5, 13];

        foreach (SavedNode savedNode in savedMap.nodes)
        {
            BaseNode node = createNode(savedNode, db);
            if (node == null) continue;

            node.gridPosition = new Vector2Int(savedNode.col, savedNode.floor);
            node.isVisited = savedNode.isVisited;
            node.isUnlocked = savedNode.isUnlocked;
            node.visitedIconPath = $"Icons_map/X_{UnityEngine.Random.Range(1, 4)}";

            nodeGrid[savedNode.col, savedNode.floor] = node;
            map.nodes.Add(node);
        }

        foreach (SavedNode savedNode in savedMap.nodes)
        {
            BaseNode node = nodeGrid[savedNode.col, savedNode.floor];
            if (node == null) continue;

            for (int i = 0; i < savedNode.connectionCols.Count; i++)
            {
                int connCol = savedNode.connectionCols[i];
                int connFloor = savedNode.connectionFloors[i];
                BaseNode connNode = nodeGrid[connCol, connFloor];
                if (connNode != null)
                    node.connections.Add(connNode);
            }
        }

        return map;
    }

    private static BaseNode createNode(SavedNode savedNode, SaveDatabase db)
    {
        switch (savedNode.nodeType)
        {
            case "Battle":
            case "Elite":
            case "Boss":
                {
                    BattleNode battleNode = new BattleNode();
                    battleNode.difficulty = (BattleDifficulty)Enum.Parse(typeof(BattleDifficulty), savedNode.difficulty);

                    List<UnitData> enemies = new List<UnitData>();
                    foreach (string enemyName in savedNode.enemyNames)
                    {
                        UnitData enemy = db.findEnemy(enemyName);
                        if (enemy != null)
                            enemies.Add(enemy);
                    }
                    battleNode.enemies = enemies.ToArray();
                    return battleNode;
                }
            case "Shop":
                return new ShopNode();
            case "Rest":
                return new RestNode();
            case "Relic":
                return new RelicNode();
            case "Event":
                {
                    EventNode eventNode = new EventNode();
                    if (!string.IsNullOrEmpty(savedNode.eventName))
                        eventNode.eventData = db.findEvent(savedNode.eventName);
                    return eventNode;
                }
            default:
                return null;
        }
    }
}