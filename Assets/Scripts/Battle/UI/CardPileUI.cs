using UnityEngine;
using TMPro;

public class CardPileUI : MonoBehaviour
{
    public CardPileSystem cardPileSystem;

    public TMP_Text drawPileCountText;
    public TMP_Text discardPileCountText;
    public TMP_Text exhaustedPileCountText;

    void OnEnable()
    {
        cardPileSystem.OnDrawPileChanged += updateDrawPileCount;
        cardPileSystem.OnDiscardPileChanged += updateDiscardPileCount;
        cardPileSystem.OnExhaustPileChanged += updateExhaustedPileCount;
    }

    void OnDisable()
    {
        cardPileSystem.OnDrawPileChanged -= updateDrawPileCount;
        cardPileSystem.OnDiscardPileChanged -= updateDiscardPileCount;
        cardPileSystem.OnExhaustPileChanged -= updateExhaustedPileCount;
    }

    void updateDrawPileCount(int count)
    {
        drawPileCountText.text = $"{count}";
    }

    void updateDiscardPileCount(int count) {
        discardPileCountText.text = $"{count}";
    }

    void updateExhaustedPileCount(int count) {
        exhaustedPileCountText.text = $"{count}";
    }
}


