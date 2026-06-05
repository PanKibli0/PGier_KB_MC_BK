using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        GameManager.OnHealthChanged += updateHealth;
        updateHealth();
    }

    private void OnDestroy()
    {
        GameManager.OnHealthChanged -= updateHealth;
    }

    private void updateHealth()
    {
        if (healthText != null && GameManager.Instance != null)
            healthText.text = $"{GameManager.Instance.currentHealth}/{GameManager.Instance.maxHealth}";
    }
}