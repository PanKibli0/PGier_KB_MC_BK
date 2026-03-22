using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitStatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject blockPanel;
    [SerializeField] private TMP_Text blockText;

    private Unit unit;


    public void init(Unit unit)
    {
        this.unit = unit;
        unit.setStatsUI(this);

        nameText.text = unit.unitName;
        updateUI();
    }


    public void updateUI()
    {
        if (unit == null) return;

        healthText.text = $"{unit.currentHealth}/{unit.maxHealth}";

        healthBar.maxValue = unit.maxHealth;
        healthBar.value = unit.currentHealth;

        if (unit.block > 0)
        {
            blockPanel.SetActive(true);
            blockText.text = unit.block.ToString();
        }
        else
        {
            blockPanel.SetActive(false);
        }
    }
}