using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class IntentMoveTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Tooltip tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Unit unit = GetComponentInParent<UnitStatsUI>().unit;
        UnitMove move = unit.nextMove;

        if (move == null || tooltip == null) return;

        BaseAction firstAction = move.actions[0];

        string description = "";
        foreach (BaseAction action in move.actions)
        {
            description += action.getCardDescription(unit, null) + "\n";
        }

        tooltip.show(new List<(Sprite, string, string)> { (firstAction.getIcon(), move.moveName, description) });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.hide();
    }
}