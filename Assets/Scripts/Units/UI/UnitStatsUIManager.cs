using UnityEngine;

public class UnitStatsUIManager : MonoBehaviour
{
    public static UnitStatsUIManager Instance;

    [SerializeField] private UnitStatsUI statsUIPrefab;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    public void createStatsUI(Unit unit)
    {

        if (statsUIPrefab == null || unit == null) return;
  
        UnitStatsUI statsUI = Instantiate(statsUIPrefab, unit.transform);
        statsUI.init(unit);
    }
}