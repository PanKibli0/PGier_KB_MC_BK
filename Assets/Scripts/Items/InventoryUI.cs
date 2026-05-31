using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform container;
    public GameObject itemPrefab;
    public Tooltip tooltip;


    private void Start()
    {
        if (PlayerInventory.Instance != null)
            PlayerInventory.Instance.OnInventoryChanged += refresh;
        
        refresh();
    }
    private void OnDestroy()
    {
        if (PlayerInventory.Instance != null)
            PlayerInventory.Instance.OnInventoryChanged -= refresh;
    }

    public void refresh()
    {
        if (PlayerInventory.Instance == null)
        {
            return;
        }

        if (container == null || itemPrefab == null)
        {
            return;
        }

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < PlayerInventory.Instance.items.Count; i++)
        {
            ItemData item = PlayerInventory.Instance.items[i];
            
            GameObject obj = Instantiate(itemPrefab, container);

            ItemClickUI ui = obj.GetComponent<ItemClickUI>();

            if (ui == null)
            {
                continue;
            }

            ui.setup(item, i);
        }
    }
}