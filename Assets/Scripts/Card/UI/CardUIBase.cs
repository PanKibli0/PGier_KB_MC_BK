using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CardUIBase : MonoBehaviour
{
    [Header("Card Data")]
    protected Card card;

    [Header("UI")]
    [SerializeField] protected Image frontImage;
    [SerializeField] protected Image cardArtImage;
    [SerializeField] protected TMP_Text nameText;
    [SerializeField] protected TMP_Text descText;
    [SerializeField] protected Image energyIcon;
    [SerializeField] protected TMP_Text costText;
    [SerializeField] protected Sprite[] frontSprites;




    public virtual void init(Card card)
    {
        this.card = card;

        if (card.data.image != null)
            cardArtImage.sprite = card.data.image;

        int typeIndex = (int)card.data.type;
        if (frontSprites != null && typeIndex < frontSprites.Length)
            frontImage.sprite = frontSprites[typeIndex];

        nameText.text = card.data.cardName;
        costText.text = $"{card.currentCost}";

        updateCostColor();
        updateDescription();
    }

    public void updateDescription(Unit target = null, bool applyEffects = false)
    {
        if (card == null || card.actions == null) return;

        string description = "";
        Unit player = (UnitsManager.Instance != null) ? UnitsManager.Instance.player : null;

        foreach (var action in card.actions)
        {
            description += action.getCardDescription(player, target, applyEffects) + "\n";
        }
        descText.text = description;
    }

    protected void updateCostColor()
    {
        if (costText == null) return;

        if (EnergySystem.Instance != null && !EnergySystem.Instance.canAfford(card.currentCost))
            costText.color = Color.red;
        else
            costText.color = Color.white;
    }
}