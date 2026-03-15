using UnityEngine;
using TMPro;

public class CardUI : MonoBehaviour
{
    private Card card;

    public TMP_Text nameText;


    public void init(Card card)
    {
        this.card = card;
        nameText.text = card.data.cardName;
        Debug.Log($"<color=cyan>Initialized CardUI for {card.data.name}.</color>");
    }
}
