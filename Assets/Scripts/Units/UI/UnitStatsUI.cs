using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnitStatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject blockPanel;
    [SerializeField] private TMP_Text blockText;

    [SerializeField] private Transform effectsContainer;
    [SerializeField] private EffectUI effectUIPrefab;

    [SerializeField] private Transform intentContainer;
    [SerializeField] private IntentMoveIcon intentIconPrefab;

    [SerializeField] private Tooltip tooltip;

    public Unit unit;

    void Start()
    {
        if (unit != null)
            unit.OnEffectsChanged += updateEffectsUI;
    }

    void OnDestroy()
    {
        if (unit != null)
            unit.OnEffectsChanged -= updateEffectsUI;
    }

    public void init(Unit unit)
    {
        this.unit = unit;
        unit.setStatsUI(this);

        if (unit != null)
            unit.OnEffectsChanged += updateEffectsUI;

        nameText.text = unit.unitName;
        updateUI();
        updateEffectsUI();

        bool isRight = (unit.unitType == UnitType.Player || unit.unitType == UnitType.Ally);
        tooltip.setPositionSide(isRight);
    }


    public void updateUI()
    {
        if (unit == null) return;

        healthText.text = $"{unit.currentHealth}/{unit.maxHealth}";

        healthBar.maxValue = unit.maxHealth;
        healthBar.value = unit.currentHealth;

        if (unit.block > 0)
        {
            blockPanel.SetActive(true);
            blockText.text = unit.block.ToString();
        }
        else
        {
            blockPanel.SetActive(false);
        }
    }

    public void updateEffectsUI()
    {
        if (effectsContainer == null) return;

        foreach (Transform child in effectsContainer)
            Destroy(child.gameObject);

        int maxDisplay = 10;
        int toShow = unit.effects.Count > maxDisplay ? maxDisplay - 1 : unit.effects.Count;

        for (int i = 0; i < toShow; i++)
        {
            EffectUI effectUI = Instantiate(effectUIPrefab, effectsContainer);
            effectUI.init(unit.effects[i], tooltip);
        }

        if (unit.effects.Count > maxDisplay)
        {
            int hiddenCount = unit.effects.Count - toShow;
            List<BaseStatusEffect> hiddenEffects = unit.effects.GetRange(toShow, hiddenCount);

            EffectUI moreUI = Instantiate(effectUIPrefab, effectsContainer);
            moreUI.initAsMore(hiddenEffects, tooltip);
        };
    }

    public void showIntent(UnitMove move)
    {
        if (intentContainer == null) return;

        if (move == null || move.actions == null || move.actions.Count == 0)
        {
            intentContainer.gameObject.SetActive(false);
            return;
        }

        foreach (Transform child in intentContainer)
            Destroy(child.gameObject);

        foreach (var action in move.actions)
        {
            IntentMoveIcon icon = Instantiate(intentIconPrefab, intentContainer);
            icon.init(action.getIconPath(), action.getValue());
        }

        intentContainer.gameObject.SetActive(true);
    }

    public void hideIntent()
    {
        if (intentContainer != null)
            intentContainer.gameObject.SetActive(false);
    }
}