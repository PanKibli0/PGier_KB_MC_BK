using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "SaveDatabase", menuName = "SaveDatabase")]
public class SaveDatabase : ScriptableObject
{
    public List<CardData> allCards;
    public List<RelicData> allRelics;
    public List<ItemData> allItems;
    public List<UnitData> allEnemies;
    public List<EventData> allEvents;
    public List<CharacterData> allCharacters;

    public CardData findCard(string name)
    {
        foreach (CardData card in allCards)
            if (card.name == name) return card;
        return null;
    }

    public RelicData findRelic(string name)
    {
        foreach (RelicData relic in allRelics)
            if (relic.name == name) return relic;
        return null;
    }

    public ItemData findItem(string name)
    {
        foreach (ItemData item in allItems)
            if (item.name == name) return item;
        return null;
    }

    public UnitData findEnemy(string name)
    {
        foreach (UnitData enemy in allEnemies)
            if (enemy.name == name) return enemy;
        return null;
    }

    public EventData findEvent(string name)
    {
        foreach (EventData ev in allEvents)
            if (ev.name == name) return ev;
        return null;
    }

    public CharacterData findCharacter(string name)
    {
        foreach (CharacterData character in allCharacters)
            if (character.name == name) return character;
        return null;
    }

#if UNITY_EDITOR
    public void findAll()
    {
        allCards = new List<CardData>();
        allRelics = new List<RelicData>();
        allItems = new List<ItemData>();
        allEnemies = new List<UnitData>();
        allEvents = new List<EventData>();
        allCharacters = new List<CharacterData>();

        string[] guids = AssetDatabase.FindAssets("t:CardData");
        foreach (string guid in guids)
        {
            CardData card = AssetDatabase.LoadAssetAtPath<CardData>(AssetDatabase.GUIDToAssetPath(guid));
            if (card != null)
                allCards.Add(card);
        }

        guids = AssetDatabase.FindAssets("t:RelicData");
        foreach (string guid in guids)
        {
            RelicData relic = AssetDatabase.LoadAssetAtPath<RelicData>(AssetDatabase.GUIDToAssetPath(guid));
            if (relic != null)
                allRelics.Add(relic);
        }

        guids = AssetDatabase.FindAssets("t:ItemData");
        foreach (string guid in guids)
        {
            ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(guid));
            if (item != null)
                allItems.Add(item);
        }

        guids = AssetDatabase.FindAssets("t:UnitData");
        foreach (string guid in guids)
        {
            UnitData enemy = AssetDatabase.LoadAssetAtPath<UnitData>(AssetDatabase.GUIDToAssetPath(guid));
            if (enemy != null)
                allEnemies.Add(enemy);
        }

        guids = AssetDatabase.FindAssets("t:EventData");
        foreach (string guid in guids)
        {
            EventData ev = AssetDatabase.LoadAssetAtPath<EventData>(AssetDatabase.GUIDToAssetPath(guid));
            if (ev != null)
                allEvents.Add(ev);
        }

        guids = AssetDatabase.FindAssets("t:CharacterData");
        foreach (string guid in guids)
        {
            CharacterData character = AssetDatabase.LoadAssetAtPath<CharacterData>(AssetDatabase.GUIDToAssetPath(guid));
            if (character != null)
                allCharacters.Add(character);
        }

        EditorUtility.SetDirty(this);
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(SaveDatabase))]
public class SaveDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SaveDatabase db = (SaveDatabase)target;

        if (GUILayout.Button("ZNAJDZ ASSETY DO WCZYTYWANIA ZAPISU"))
            db.findAll();
    }
}
#endif