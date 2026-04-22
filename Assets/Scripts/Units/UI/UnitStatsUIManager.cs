using UnityEngine;

public class UnitStatsUIManager : MonoBehaviour
{
    [SerializeField] private UnitStatsUI statsUIPrefab;


    public void createStatsUI(Unit unit)
    {

        if (statsUIPrefab == null || unit == null) return;
  
        UnitStatsUI statsUI = Instantiate(statsUIPrefab, unit.transform);
        statsUI.init(unit);
    }
}