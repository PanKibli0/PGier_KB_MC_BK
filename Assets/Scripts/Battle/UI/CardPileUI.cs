using UnityEngine;
using TMPro;

public class CardPileUI : MonoBehaviour
{
    private CardPileSystem cardPileSystem;

    [SerializeField] private TMP_Text drawPileCountText;
    [SerializeField] private TMP_Text discardPileCountText;
    [SerializeField] private TMP_Text exhaustedPileCountText;
    [SerializeField] private GameObject exhaustPanel;

    public void init(CardPileSystem cardPileSystem)
    {
        this.cardPileSystem = cardPileSystem;
        cardPileSystem.OnDrawPileChanged += updateDrawPileCount;
        cardPileSystem.OnDiscardPileChanged += updateDiscardPileCount;
        cardPileSystem.OnExhaustPileChanged += updateExhaustedPileCount;

        exhaustPanel.SetActive(false);
    }

    void OnDestroy()
    {
        if (cardPileSystem != null)
        {
            cardPileSystem.OnDrawPileChanged -= updateDrawPileCount;
            cardPileSystem.OnDiscardPileChanged -= updateDiscardPileCount;
            cardPileSystem.OnExhaustPileChanged -= updateExhaustedPileCount;
        }
    }

    void updateDrawPileCount(int count)
    {
        drawPileCountText.text = $"{count}";
    }

    void updateDiscardPileCount(int count)
    {
        discardPileCountText.text = $"{count}";
    }

    void updateExhaustedPileCount(int count)
    {
        exhaustedPileCountText.text = $"{count}";
        exhaustPanel.SetActive(count > 0);
    }
}