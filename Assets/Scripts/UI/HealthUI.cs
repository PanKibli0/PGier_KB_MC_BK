using UnityEngine;
using TMPro;

public class GlobalHealthBar : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    private void OnEnable()
    {
        GameManager.OnHealthChanged += updateHealth;
        updateHealth();
    }

    private void OnDisable()
    {
        GameManager.OnHealthChanged -= updateHealth;
    }

    private void updateHealth()
    {
        if (healthText != null && GameManager.Instance != null)
            healthText.text = $"{GameManager.Instance.currentHealth}/{GameManager.Instance.maxHealth}";
    }
}