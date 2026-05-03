using UnityEngine;
using TMPro;

public class PriceTag : MonoBehaviour
{
    [SerializeField] private TMP_Text priceText;
    public int price;

    private void OnEnable()
    {
        GameManager.OnGoldChanged += updateColor;
    }

    private void OnDisable()
    {
        GameManager.OnGoldChanged -= updateColor;
    }

    public void setPrice(int value)
    {
        price = value;
        priceText.text = $"{price}";
        updateColor(GameManager.Instance.gold);
    }

    private void updateColor(int currentGold)
    {
        priceText.color = currentGold >= price ? Color.white : Color.red;
    }
}