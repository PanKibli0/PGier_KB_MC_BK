using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private TMP_Text countText;

    private PlayerInventory inventory;
    private Unit playerUnit;

    private void Start()
    {
        inventory = GameManager.Instance.playerInventory;
        inventory.onInventoryChanged += refresh;
        refresh();
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.onInventoryChanged -= refresh;
    }

    public void setPlayerUnit(Unit unit)
    {
        playerUnit = unit;
        refresh();
    }

    public void refresh()
    {
        if (inventory == null || container == null || itemPrefab == null) return;

        foreach (Transform child in container)
            Destroy(child.gameObject);

        for (int i = 0; i < inventory.items.Count; i++)
        {
            GameObject obj = Instantiate(itemPrefab, container);
            ItemSlotUI ui = obj.GetComponent<ItemSlotUI>();
            if (ui == null) continue;
            ui.setup(inventory.items[i], i, tooltip, inventory, playerUnit);
        }

        if (countText != null)
            countText.text = $"{inventory.items.Count}/{inventory.maxItems}";
    }
}