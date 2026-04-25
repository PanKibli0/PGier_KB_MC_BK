using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform container;
    public GameObject itemPrefab;
    public Tooltip tooltip;


    private void Start()
    {
        PlayerInventory.Instance.OnInventoryChanged += refresh;
        refresh();
    }

    public void refresh()
    {
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