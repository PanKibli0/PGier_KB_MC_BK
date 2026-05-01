using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemDatabase))]
public class ItemDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ItemDatabase database = (ItemDatabase)target;

        if (GUILayout.Button("Load All Items"))
        {
            LoadAllItems(database);
        }
    }

    private void LoadAllItems(ItemDatabase database)
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemData");

        database.items.Clear();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);

            if (item != null)
                database.items.Add(item);
        }

        EditorUtility.SetDirty(database);
        Debug.Log("Za³adowano itemy: " + database.items.Count);
    }
}