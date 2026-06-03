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

    protected EnergySystem energySystem;
    protected UnitsManager unitsManager;

    public virtual void init(Card card, EnergySystem energy = null, UnitsManager units = null)
    {
        this.card = card;
        this.energySystem = energy;
        this.unitsManager = units;

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
        Unit player = (unitsManager != null) ? unitsManager.player : null;

        foreach (var action in card.actions)
        {
            description += $"{action.getCardDescription(player, target, applyEffects)} {TargetingSystem.getTargetText(action.targetType)}\n";
        }
        descText.text = description;
    }

    protected void updateCostColor()
    {
        if (costText == null) return;

        if (energySystem == null)
        {
            Debug.Log("energySystem is null for card: " + card?.data?.cardName);
            return;
        }

        if (energySystem != null && !energySystem.canAfford(card.currentCost))
            costText.color = Color.red;
        else
            costText.color = Color.white;
    }
}