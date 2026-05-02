using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGoldChanged += updateGold;
            updateGold(GameManager.Instance.gold);
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGoldChanged -= updateGold;
        }
    }

    private void updateGold(int gold)
    {
        goldText.text = gold.ToString();
    }
}