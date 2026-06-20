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

    private RelicManager relics;
    private CardPileSystem cardPileSystem;
    private HandSystem handSystem;


    public void init(Card card, EnergySystem energy, CardPileSystem cards, HandSystem hand, UnitsManager units, RelicManager relic)
    {
        base.init(card, energy, units);
        this.cardPileSystem = cards;
        this.handSystem = hand;
        this.relics = relic;

        updateDescription(null, true);

        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        handArea = GetComponentInParent<HandAreaUI>();

        this.energySystem.OnEnergyChanged += updateCostColor;
        this.unitsManager.OnUnitsChanged += updateCostColor;
        this.unitsManager.player.OnEffectsChanged += onEffectsChanged;
    }


    protected virtual void OnDestroy()
    {
        if (this.energySystem != null)
            this.energySystem.OnEnergyChanged -= updateCostColor;

        if (this.unitsManager != null && this.unitsManager.player != null)
            this.unitsManager.player.OnEffectsChanged -= onEffectsChanged;
    }


    protected void onEffectsChanged() { updateDescription(); }


    private bool canPlayCard()
    {
        if (!energySystem.canAfford(card.currentCost)) return false;

        foreach (var action in card.actions)
        {
            if (action is SummonAction summon)
            {
                UnitType summonedType = (unitsManager.player.unitType == UnitType.Player) ? UnitType.Ally : UnitType.Enemy;

                if (summonedType == UnitType.Ally && !unitsManager.canSummonAlly()) return false;
                if (summonedType == UnitType.Enemy && !unitsManager.canSummonEnemy()) return false;
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
            updateDescription(currentHoverTarget, true);
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
        updateDescription(null, true);

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
            if (targetUnit == null) continue;

            bool valid = true;
            foreach (var action in card.actions)
            {
                if (!action.requiresTarget()) continue;
                if (!TargetingSystem.isValidTarget(unitsManager.player, action.targetType, targetUnit))
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
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
        Unit player = unitsManager.player;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.cardPlay);
        // Wybór animacji na podstawie akcji karty
        foreach (var action in card.actions)
        {
            if (action is DamageAction || action is TrueDamageAction)
                player.PlayAnimation("Attack");

            else if (action is BlockAction)
                player.PlayAnimation("Defence");

            else if (action is HealAction)
                player.PlayAnimation("Special");

            else if (action is SummonAction)
                player.PlayAnimation("Special");

            else if (action is RemoveEffectAction)
                player.PlayAnimation("Special");
        }

        // Dalej zostawiasz istniej¹cy kod
        energySystem.spendEnergy(card.currentCost);

        foreach (var action in card.actions)
        {
            if (action is SummonAction)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.summon);
            }
            List<Unit> targets = TargetingSystem.getTargets(player, action.targetType, selectedTarget);

            foreach (Unit target in targets)
            {
                if (target != null)
                {
                    target.PlayAnimation("HURT");
                    action.execute(target, player);
                }
            }
        }

        relics.onCardPlayed(player, card);

        for (int i = player.effects.Count - 1; i >= 0; i--)
            player.effects[i].onCardPlayed(player);

        if (card.data.exhaust)
            cardPileSystem.exhaustCard(card);
        else
            cardPileSystem.discardCard(card);

        handSystem.hand.Remove(card);

        Destroy(gameObject);
    }


    private void returnToHand()
    {
        transform.SetParent(startParent);
        transform.SetSiblingIndex(startSiblingIndex);
        handArea?.refreshLayout();
    }

    protected new void updateCostColor()
    {
        if (costText == null) return;
        costText.color = canPlayCard() ? Color.white : Color.red;
    }
}