using System;
using System.Collections.Generic;

public class PlayerInventory
{
    public List<ItemData> items = new List<ItemData>();
    public int maxItems = 3;

    public event Action onInventoryChanged;

    public bool addItem(ItemData item)
    {
        if (item == null) return false;
        if (items.Count >= maxItems) return false;

        items.Add(item);
        onInventoryChanged?.Invoke();
        return true;
    }

    public void removeItem(ItemData item)
    {
        if (item == null) return;
        items.Remove(item);
        onInventoryChanged?.Invoke();
    }

    public void removeItemAt(int index)
    {
        if (index < 0 || index >= items.Count) return;
        items.RemoveAt(index);
        onInventoryChanged?.Invoke();
    }

    public void useItem(int index, Unit player, Unit selectedTarget = null)
    {
        if (index < 0 || index >= items.Count) return;
        ItemData item = items[index];
        if (item == null || player == null) return;

        item.use(player, selectedTarget);
        items.RemoveAt(index);
        onInventoryChanged?.Invoke();
    }
}
