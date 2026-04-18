using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CardUIPlayable : CardUIBase, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Drag And Drop System")]
    private RectTransform rectTransform;
    private Canvas canvas;
    private Transform startParent;
    private int startSiblingIndex;
    private HandAreaUI handArea;
    private Unit selectedTarget;
    private Unit currentHoverTarget;
    private bool canDrag = true;

    protected void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        handArea = GetComponentInParent<HandAreaUI>();
    }

    private bool canPlayCard()
    {
        if (EnergySystem.Instance != null && !EnergySystem.Instance.canAfford(card.currentCost)) return false;

        foreach (var action in card.actions)
        {
            if (action is SummonAction summon)
            {
                UnitType summonedType = (UnitsManager.Instance.player.unitType == UnitType.Player) ? UnitType.Ally : UnitType.Enemy;

                if (summonedType == UnitType.Ally && !UnitsManager.Instance.canSummonAlly()) return false;
                if (summonedType == UnitType.Enemy && !UnitsManager.Instance.canSummonEnemy()) return false;
            }
        }
        return true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canPlayCard())
        {
            canDrag = false;
            return;
        }

        canDrag = true;
        startParent = transform.parent;
        startSiblingIndex = transform.GetSiblingIndex();

        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0f;
        transform.position = worldPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0f;
        transform.position = worldPos;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        Unit newTarget = null;
        foreach (var result in results)
        {
            newTarget = result.gameObject.GetComponent<Unit>();
            if (newTarget != null) break;
        }

        if (currentHoverTarget != newTarget)
        {
            currentHoverTarget = newTarget;
            updateDescription(currentHoverTarget);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            canDrag = true;
            return;
        }

        currentHoverTarget = null;
        updateDescription(null);

        bool canPlay;

        if (cardRequiresTarget())
            canPlay = canPlayWithTarget(eventData);
        else
            canPlay = isOverPlayArea(eventData);

        if (canPlay)
            playCard();
        else
            returnToHand();
    }


    private bool cardRequiresTarget()
    {
        if (card == null || card.actions == null) return false;

        foreach (var action in card.actions)
        {
            if (action.requiresTarget()) return true;
        }
        return false;
    }


    private bool canPlayWithTarget(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            Unit targetUnit = result.gameObject.GetComponent<Unit>();
            if (targetUnit != null)
            {
                selectedTarget = targetUnit;
                return true;
            }
        }

        selectedTarget = null;
        return false;
    }


    private bool isOverPlayArea(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("PlayArea")) return true;
        }
        return false;
    }


    private void playCard()
    {
        if (EnergySystem.Instance != null)
            EnergySystem.Instance.spendEnergy(card.currentCost);

        foreach (var action in card.actions)
        {
            List<Unit> targets = TargetingSystem.getTargets(UnitsManager.Instance.player, action.targetType, selectedTarget);

            foreach (Unit target in targets)
                if (target != null)
                    action.execute(target, UnitsManager.Instance.player);
        }

        HandSystem.Instance.removeCard(card);
        Destroy(gameObject);
    }

    private void returnToHand()
    {
        transform.SetParent(startParent);
        transform.SetSiblingIndex(startSiblingIndex);
        handArea?.refreshLayout();
    }
}