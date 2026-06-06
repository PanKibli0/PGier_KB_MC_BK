using UnityEngine;

public class MainBar : MonoBehaviour
{
    public static MainBar Instance;

    [SerializeField] private InventoryUI inventoryUI;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void setPlayerUnit(Unit unit)
    {
        inventoryUI?.setPlayerUnit(unit);
    }
}
