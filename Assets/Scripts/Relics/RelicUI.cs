using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class RelicUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image iconImage;

    private RelicData relicData;
    private Tooltip tooltip;
    private RectTransform rectTransform;
    private RelicRewardPanel rewardPanel;
    private RelicSceneManager sceneManager;
    private bool positionTooltip;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void setup(RelicData relic, Tooltip tooltipRef)
    {
        relicData = relic;
        tooltip = tooltipRef;
        rewardPanel = null;
        sceneManager = null;
        positionTooltip = true;

        if (iconImage != null && relicData.icon != null)
            iconImage.sprite = relicData.icon;
    }

    public void setupReward(RelicData relic, RelicRewardPanel panel, Tooltip tooltipRef)
    {
        relicData = relic;
        tooltip = tooltipRef;
        rewardPanel = panel;
        sceneManager = null;
        positionTooltip = false;

        if (iconImage != null && relicData.icon != null)
            iconImage.sprite = relicData.icon;
    }

    public void setupScene(RelicData relic, RelicSceneManager manager, Tooltip tooltipRef)
    {
        relicData = relic;
        tooltip = tooltipRef;
        sceneManager = manager;
        rewardPanel = null;
        positionTooltip = false;

        if (iconImage != null && relicData.icon != null)
            iconImage.sprite = relicData.icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (rewardPanel != null)
            rewardPanel.selectRelic(relicData);
        else if (sceneManager != null)
            sceneManager.selectRelic(relicData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (relicData == null || tooltip == null) return;

        string triggerInfo = $"<color=yellow>{getTriggerText()}</color>";
        string desc = "";
        foreach (var action in relicData.actions)
        {
            if (action != null)
                desc += $"{action.getCardDescription()} {TargetingSystem.getTargetText(action.targetType)}\n";
        }

        var entries = new List<(Sprite, string, string)>
        {
            (relicData.icon, relicData.relicName, $"{triggerInfo}\n{desc}")
        };

        if (positionTooltip)
        {
            tooltip.show(entries);
            Vector3 pos = transform.position;
            pos.y -= rectTransform.rect.height;
            float tooltipWidth = tooltip.GetComponent<RectTransform>().rect.width;
            if (pos.x + tooltipWidth > Screen.width)
                pos.x -= tooltipWidth;
            tooltip.transform.position = pos;
        }
        else
        {
            Vector3 savedPos = tooltip.transform.position;
            tooltip.show(entries);
            tooltip.transform.position = savedPos;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.hide();
    }

    private string getTriggerText()
    {
        switch (relicData.trigger)
        {
            case RelicTrigger.OnBattleStart: return "NA POCZATKU WALKI";
            case RelicTrigger.OnBattleEnd: return "PO WALCE";
            case RelicTrigger.OnTurnStart: return "NA POCZATKU TURY";
            case RelicTrigger.OnTurnEnd: return "NA KONCU TURY";
            case RelicTrigger.OnCardPlayed: return "PO ZAGRANIU KARTY";
            case RelicTrigger.OnDamageDealt: return "PO ZADANIU OBRAZEN";
            case RelicTrigger.OnDamageTaken: return "PO OTRZYMANIU OBRAZEN";
            default: return "";
        }
    }
}