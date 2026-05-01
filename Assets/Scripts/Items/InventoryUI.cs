using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform container;
    public GameObject itemPrefab;
    public Tooltip tooltip;


    private void Start()
    {
        Debug.Log("SUBSCRIBED UI: " + gameObject.name);
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
        Debug.Log($"InventoryUI instance: {gameObject.name}");
        Debug.Log($"container: {container}");
        Debug.Log($"itemPrefab: {itemPrefab}");
        if (PlayerInventory.Instance == null)
        {
            Debug.LogError("PlayerInventory.Instance == NULL - brak obiektu w scenie!");
            return;
        }

        if (container == null || itemPrefab == null)
        {
            Debug.LogError("UI refs missing!");
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
                Debug.LogError("ItemPrefab nie ma ItemClickUI!");
                continue;
            }

            ui.setup(item, i);
        }
    }
}