using System;
using System.Collections.Generic;

public class PlayerInventory
{
    public List<ItemData> items = new List<ItemData>();
    public int maxItems = 3;

    public event Action onInventoryChanged;

    public bool isFull()
    {
        return items.Count >= maxItems;
    }

    public bool canAddItem(ItemData item)
    {
        if (item == null) return false;
        return !isFull();
    }

    public bool addItem(ItemData item)
    {
        if (item == null) return false;
        if (isFull()) return false;

        items.Add(item);
        onInventoryChanged?.Invoke();
        return true;
    }

    public bool removeItem(ItemData item)
    {
        if (item == null) return false;

        bool removed = items.Remove(item);
        if (removed)
            onInventoryChanged?.Invoke();

        return removed;
    }

    public bool removeItemAt(int index)
    {
        if (index < 0 || index >= items.Count)
            return false;

        items.RemoveAt(index);
        onInventoryChanged?.Invoke();
        return true;
    }

    public bool useItem(int index, Unit player, Unit selectedTarget = null)
    {
        if (index < 0 || index >= items.Count) return false;
        if (player == null) return false;

        ItemData item = items[index];
        if (item == null) return false;

        item.use(player, selectedTarget);
        items.RemoveAt(index);
        onInventoryChanged?.Invoke();

        return true;
    }
}