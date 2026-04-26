using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    public ItemData testItem;
    public List<ItemData> items = new List<ItemData>();

    public event System.Action OnInventoryChanged;

    void Start()
    {
        AddItem(testItem);
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UseItem(int index)
    {
        UseItem(index, null);
    }

    public void UseItem(int index, Unit selectedTarget)
    {
        if (index < 0 || index >= items.Count)
            return;

        ItemData item = items[index];

        if (item == null)
            return;

        Unit player = UnitsManager.Instance.player;

        item.Use(player, selectedTarget);

        items.RemoveAt(index);
        OnInventoryChanged?.Invoke();
    }
    public void AddItem(ItemData item)
    {
        if (item == null) return;

        items.Add(item);
        OnInventoryChanged?.Invoke();
    }
    public void RemoveItem(ItemData item)
    {
        if (item == null) return;

        items.Remove(item);
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItemAt(int index)
    {
        if (index < 0 || index >= items.Count)
            return;

        items.RemoveAt(index);
        OnInventoryChanged?.Invoke();
    }
}