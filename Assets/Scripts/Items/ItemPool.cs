using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemPool", menuName = "Game/ItemPool")]
public class ItemPool : ScriptableObject
{
    public List<ItemData> items = new();

    public ItemData GetRandomItem()
    {
        if (items == null || items.Count == 0)
            return null;

        return items[Random.Range(0, items.Count)];
    }
}