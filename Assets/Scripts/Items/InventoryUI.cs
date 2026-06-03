using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform container;
    public GameObject itemPrefab;
    public Tooltip tooltip;

    private PlayerInventory inventory;
    private Unit playerUnit;

    public void init(PlayerInventory inventory, Unit playerUnit)
    {
        this.inventory = inventory;
        this.playerUnit = playerUnit;

        inventory.onInventoryChanged += refresh;
        refresh();
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.onInventoryChanged -= refresh;
    }

    public void refresh()
    {
        if (inventory == null) return;
        if (container == null || itemPrefab == null) return;

        foreach (Transform child in container)
            Destroy(child.gameObject);

        for (int i = 0; i < inventory.items.Count; i++)
        {
            ItemData item = inventory.items[i];
            GameObject obj = Instantiate(itemPrefab, container);
            ItemClickUI ui = obj.GetComponent<ItemClickUI>();
            if (ui == null) continue;

            ui.setup(item, i, null, inventory, playerUnit);
        }
    }
}