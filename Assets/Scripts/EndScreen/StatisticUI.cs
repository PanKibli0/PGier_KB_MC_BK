using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI enemiesText;
    public TextMeshProUGUI floorsText;

    private void Start()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("No GameManager");
            return;
        }

        goldText.text = "Z³oto: " + GameManager.Instance.gold;
        enemiesText.text = "Zabici przeciwnicy: " + GameManager.Instance.enemiesKilled;
        floorsText.text = "Pokonane poziomy: " + GameManager.Instance.floorsCompleted;
        
    }
}