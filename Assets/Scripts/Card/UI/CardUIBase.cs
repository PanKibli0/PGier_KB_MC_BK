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

    protected virtual void Start()
    {
        if (EnergySystem.Instance != null)
            EnergySystem.Instance.OnEnergyChanged += updateCostColor;

        UnitsManager.Instance.player.OnEffectsChanged += onEffectsChanged;
    }

    protected virtual void OnDestroy()
    {
        if (EnergySystem.Instance != null)
            EnergySystem.Instance.OnEnergyChanged -= updateCostColor;

        if (UnitsManager.Instance?.player != null)
            UnitsManager.Instance.player.OnEffectsChanged -= onEffectsChanged;
    }

    private void onEffectsChanged() { updateDescription(); }

    public virtual void init(Card card)
    {
        this.card = card;

        if (card.data.image != null)
            cardArtImage.sprite = card.data.image;

        int typeIndex = (int)card.data.type;
        if (frontSprites != null && typeIndex < frontSprites.Length)
            frontImage.sprite = frontSprites[typeIndex];

        nameText.text = card.data.cardName;
        descText.text = $"{card.data.cost} Energy\n{string.Join("\n", card.data.actions)}";
        costText.text = $"{card.currentCost}";

        updateCostColor();
        updateDescription();
    }

    public void updateDescription(Unit target = null)
    {
        if (card == null || card.actions == null) return;

        string description = "";
        foreach (var action in card.actions)
        {
            description += action.getCardDescription(UnitsManager.Instance.player, target) + "\n";
        }
        descText.text = description;
    }

    private void updateCostColor()
    {
        if (costText == null) return;

        if (EnergySystem.Instance != null && EnergySystem.Instance.canAfford(card.currentCost))
            costText.color = Color.white;
        else
            costText.color = Color.red;
    }
}