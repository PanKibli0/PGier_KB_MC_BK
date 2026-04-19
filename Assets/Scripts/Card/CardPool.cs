#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardPool", menuName = "CardPool")]
public class CardPool : ScriptableObject
{
    public string folderPath = "Assets/Characters/Cards";
    public List<CardData> cards;

#if UNITY_EDITOR
    public void findCardsInFolder()
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.Log("Folder path is empty");
            return;
        }

        cards = new List<CardData>();
        string[] guids = AssetDatabase.FindAssets("t:CardData", new[] { folderPath });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            CardData card = AssetDatabase.LoadAssetAtPath<CardData>(path);
            if (card != null && !cards.Contains(card))
                cards.Add(card);
        }
        EditorUtility.SetDirty(this);
        Debug.Log($"Found {cards.Count} cards in {folderPath}");
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(CardPool))]
public class CardPoolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CardPool pool = (CardPool)target;

        if (GUILayout.Button("Dodaj karty z folderu"))
        {
            pool.findCardsInFolder();
        }
    }
}
#endif