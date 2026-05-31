using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class RelicUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;

    private RelicData relicData;
    private Tooltip tooltip;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void setup(RelicData relic, Tooltip tooltipRef)
    {
        relicData = relic;
        tooltip = tooltipRef;

        if (iconImage != null && relicData.icon != null)
            iconImage.sprite = relicData.icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (relicData == null || tooltip == null) return;

        List<(Sprite, string, string)> entries = new List<(Sprite, string, string)>();

        string triggerInfo = $"<color=yellow>{getTriggerText()}</color>";

        foreach (var action in relicData.actions)
        {
            if (action != null)
            {
                string targetText = TargetingSystem.getTargetText(action.targetType);
                string actionDesc = action.getCardDescription();
                entries.Add((relicData.icon, relicData.relicName, $"{triggerInfo}\n{actionDesc} {targetText}"));
            }
        }

        tooltip.show(entries);

        Vector3 relicWorldPos = transform.position;
        Vector3 tooltipPos = relicWorldPos;
        tooltipPos.y -= GetComponent<RectTransform>().rect.height;

        float tooltipWidth = tooltip.GetComponent<RectTransform>().rect.width;
        if (relicWorldPos.x + tooltipWidth > Screen.width)
            tooltipPos.x = relicWorldPos.x - tooltipWidth;

        tooltip.transform.position = tooltipPos;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.hide();
    }

    private string getTriggerText()
    {
        switch (relicData.trigger)
        {
            case RelicTrigger.OnBattleStart: return "NA POCZĄTKU WALKI";
            case RelicTrigger.OnBattleEnd: return "PO WALCE";
            case RelicTrigger.OnTurnStart: return "NA POCZĄTKU TURY";
            case RelicTrigger.OnTurnEnd: return "NA KOŃCU TURY";
            case RelicTrigger.OnCardPlayed: return "PO ZAGRANIU KARTY";
            case RelicTrigger.OnDamageDealt: return "PO ZADANIU OBRAŻEŃ";
            case RelicTrigger.OnDamageTaken: return "PO OTRZYMANIU OBRAŻEŃ";
            default: return "";
        }
    }

    
}